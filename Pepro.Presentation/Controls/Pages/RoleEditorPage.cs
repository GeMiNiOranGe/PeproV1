using System.ComponentModel;
using Pepro.Business;
using Pepro.Business.Contracts;
using Pepro.Presentation.Controls.Templates;
using Pepro.Presentation.Enums;
using Pepro.Presentation.Extensions;
using Pepro.Presentation.Interfaces;
using Pepro.Presentation.Utilities;

namespace Pepro.Presentation.Controls.Pages;

public partial class RoleEditorPage
    : EditorTemplate,
        IEditorUserControl<RoleDto>
{
    private RoleDto _item = null!;
    private EditorMode _mode;

    public RoleEditorPage()
    {
        InitializeComponent();

        saveButton.ApplyFlatStyle();
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public RoleDto Item
    {
        get => _item;
        set
        {
            _item = value ?? throw new ArgumentNullException(nameof(Item));
            roleNameTextBoxField.Text = _item.Name;
        }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EditorMode Mode
    {
        get => _mode;
        set
        {
            _mode = value;
            HeaderText = _mode switch
            {
                EditorMode.Create => "Create a new role",
                EditorMode.Edit => "Edit role",
                _ => throw new InvalidEnumArgumentException(
                    nameof(Mode),
                    (int)_mode,
                    typeof(EditorMode)
                ),
            };
        }
    }

    private void RoleEditorPage_Load(object sender, EventArgs e)
    {
        switch (_mode)
        {
            case EditorMode.Create:
            {
                SetupCreateMode();
                break;
            }
            case EditorMode.Edit:
            {
                SetupEditMode();
                break;
            }
            default:
            {
                throw new InvalidEnumArgumentException(
                    nameof(Mode),
                    (int)_mode,
                    typeof(EditorMode)
                );
            }
        }
    }

    private static void SetupCreateMode()
    {
        // lines of code for create mode
    }

    private static void SetupEditMode()
    {
        // lines of code for edit mode
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        if (!ValidateInputs())
        {
            MessageBoxWrapper.ShowInformation("FillInformation");
            return;
        }

        RoleDto project = new()
        {
            RoleId = _item.RoleId,
            Name = roleNameTextBoxField.Text.Trim(),
        };

        int result = _mode switch
        {
            EditorMode.Create => RoleBusiness.Instance.InsertRole(project),
            EditorMode.Edit => RoleBusiness.Instance.UpdateRole(project),
            _ => throw new InvalidEnumArgumentException(
                nameof(Mode),
                (int)_mode,
                typeof(EditorMode)
            ),
        };

        if (result > 0)
        {
            NotifyDataChanged();
            Close();
            MessageBoxWrapper.ShowInformation("SaveSuccess");
        }
        else
        {
            MessageBoxWrapper.ShowError("SaveFailed");
        }
    }

    private bool ValidateInputs()
    {
        return !string.IsNullOrWhiteSpace(roleNameTextBoxField.Text);
    }
}
