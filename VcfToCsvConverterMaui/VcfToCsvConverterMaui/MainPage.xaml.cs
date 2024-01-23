namespace VcfToCsvConverterMaui;
using Microsoft.Maui.Storage;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private async void OnConvertClicked(object sender, EventArgs e)
	{
        FilePickerFileType customFileTypeOpen = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.WinUI, new[] { ".vcf" } },
        });

        PickOptions optionsOpen = new PickOptions
		{
			PickerTitle = "Please select a contacts file",
			FileTypes = customFileTypeOpen,
		};

		var openResult = FilePicker.PickAsync(optionsOpen);
		var res = await openResult;

		if (res != null)
		{
			try
			{
				string csvContent = Converter.ConvertVcfFileToCsvContent(res.FullPath);
				var exportPath = res.FullPath.Substring(0, res.FullPath.Length - 4) + ".csv";
				if (File.Exists(exportPath))
				{
                    var answer = await Application.Current.MainPage.DisplayPromptAsync(
						"The file exists", 
						"The CSV file already exists. Edit the filename if you do not want to replace it or Cancel to abort.", 
						"OK", "Cancel", null, -1, null, exportPath);
					exportPath = answer;
                }

                if (exportPath != null)
                {
                    File.WriteAllText(exportPath, csvContent);
					await Application.Current.MainPage.DisplayAlert("CSV file created", "The CSV file was created at: " + exportPath, "OK");
                }
            } catch (Exception ex)
			{
                await Application.Current.MainPage.DisplayAlert("Unable to Convert the file", "Check that the file is correct. " + ex.Message, "OK");
            }
		}

		SemanticScreenReader.Announce(ConvertBtn.Text);
	}
}

