# EncryptedConfigEditor

EncryptedConfigEditor is a Windows Forms application designed to edit encrypted sections of a `.config` file. It provides a user-friendly interface to load, edit, and save configuration files, ensuring that sensitive sections are properly encrypted. This project was created as an alternative to the `aspnet_regiis` command line utility.

## Features

- Load and edit `.config` files by section.
- Edit contents with XML syntax highlighting.
- Automatically encrypt sections when saving.

## Prerequisites

- .NET Framework 4.7.2 or later

## Usage

1. **Browse for Config File**: Click the `Browse` button to select a `.config` file.
2. **Select a Section**: Choose a section from the dropdown to view and edit its contents.
3. **Edit Content**: Modify the XML content in the text area. Changes are tracked automatically.
4. **Save Changes**: Click the `Save Changes` button to save your modifications. The application will encrypt the section if necessary.
5. **Unsaved Changes**: If there are unsaved changes, the application will prompt you to save before closing or loading a new file.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the GNU GPLv3 License. See the [LICENSE](LICENSE) file for details.

## Contact

For any questions or suggestions, feel free to open an issue.