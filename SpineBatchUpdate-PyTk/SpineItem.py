class Spine_item:

    def __init__(self, is_folder, display_name) -> None:

        self._display_name = display_name
        self._item_path = ""

        self._is_folder = is_folder
        self.children = []

        self._is_checked = False
        self._is_selected = False
        self._is_expanded = True
    
    #region Properties

    @property
    def display_name(self) -> str:
        return self._display_name
    @display_name.setter
    def display_name(self, val):
        self._display_name = val

    @property
    def item_path(self) -> str:
        return self._item_path
    @item_path.setter
    def item_path(self, val):
        self._item_path = val

    @property
    def is_folder(self) -> bool:
        return self._is_folder
    @is_folder.setter
    def is_folder(self, val):
        self._is_folder = val

    @property
    def is_checked(self) -> bool:
        return self._is_checked
    @is_checked.setter
    def is_checked(self, val):
        self._is_checked = val

    @property
    def is_selected(self) -> bool:
        return self._is_selected
    @is_selected.setter
    def is_selected(self, val):
        self._is_selected = val
    
    @property
    def is_expanded(self) -> bool:
        return self._is_expanded
    @is_expanded.setter
    def is_expanded(self, val):
        self._is_expanded = val

    #endregion

    def __str__(self) -> str:
        str_is_folder = "Folder" if self.is_folder else "File"
        str_children = ""
        for item in self.children:
            str_children += item.__str__()
        return str_is_folder + " " + self.display_name + "\n" + str_children #"@" + self.item_path