using System;

namespace UI
{
    public interface ISelectLevelPanelUI
    {
        event Action<string> OnLevelSelected;
        void CreateLevelButtons(string[] levels);
    }
}