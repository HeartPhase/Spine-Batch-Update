from SpineItem import Spine_item
import os

class Spine_tree_gen:
    
    def __init__(self) -> None:
        self.log = []

    @staticmethod
    def generate_tree(path) -> Spine_item:
        dir = os.walk(path)

        root = next(dir)
        root_item = Spine_item(True, root[0])
        root_item.item_path = root
        for folder in root[1]:
            folder_item = Spine_tree_gen.generate_tree(folder)
            folder_item.item_path = os.path.join(root[0], folder)
            root_item.children.append(folder_item)
        for file in root[2]:
            if file.endswith(".spine"):
                file_item = Spine_item(False, file)
                file_item.item_path = os.path.join(root[0], file)
                root_item.children.append(file_item)
        return root_item
        # root_item = Spine_item(True, dir[0][0])
        # for root, folders, files in dir:
        #     for folder in folders:
        #         root_item.children.append(Spine_tree_gen.generate_tree(folder))
        #     for file in files:
        #         root_item.children.append(Spine_item(False, file[2]))