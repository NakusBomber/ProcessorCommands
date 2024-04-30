namespace ProcessorCommands.Properties {
    
    
    // Этот класс позволяет обрабатывать определенные события в классе параметров:
    //  Событие SettingChanging возникает перед изменением значения параметра.
    //  Событие PropertyChanged возникает после изменения значения параметра.
    //  Событие SettingsLoaded возникает после загрузки значений параметров.
    //  Событие SettingsSaving возникает перед сохранением значений параметров.
    internal sealed partial class Settings {
        
        public Settings() {
            this.PropertyChanged += PropertyChangedEventHandler;
        }

        private void PropertyChangedEventHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Default.Save();
        }

    }
}
