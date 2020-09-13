# >> SystemParser Class Definition Version\\Rendition 2.00  || (C) M.K. 2016 << #

import sys # Import system so reference to OS library can be made. IronPython doesn't store python libraries.
sys.path.append('\\'.join([sys.path[1], '..', 'PyScripts', 'os'])) # Add path to os library in current filesystem.
import os # Imports os module and references to all sub contained objects and methods within the os module.

class SystemParser(object):
    """Class To Recursively Parse And Find All Files In a Given Directory"""
    class SystemParserExceptions(object):
        """Exceptions utilized by SystemParser"""
        class UnidentifiableKeyException(TypeError):
            """Thrown When The Search Function Is Utilized With an Unknown Arg"""
            pass

    class PathWrapper(object):
        """Holder Class To Wrap Around SystemParser Class"""
        def generate_contents(self, path, key):
            """Generates Contents (Dirs & Files) Of A Given System"""
            for entry in os.listdir(path):
                if (key(os.path.join(path, entry))):
                    yield entry

        def generate_dirs(self, path):
            """Generates All Directories In a Given System"""
            for X in self.generate_contents(path, self.dir_parser):
                yield X
                
        def generate_files(self, path):
            """Generates All Files In a Given System"""
            for X in self.generate_contents(path, self.file_parser):
                yield X
        
        get_dirs = lambda self, path: [X for X in self.generate_contents(path, self.dir_parser)]
        get_files = lambda self, path: [X for X in self.generate_contents(path, self.file_parser)]
        dir_parser = staticmethod(os.path.isdir)
        file_parser = staticmethod(os.path.isfile)
        
    def __init__(self, work_path=None):
        """Default constructor can build system around a given path"""
        if (work_path != None):
            self.initialise(work_path)

    def __str__(self):
        """Converts instance of current class to a string object"""
        return "SystemTraverser Object Of Path : {0} :".format(self.root_path)

    def __call__(self, index=None):
        """Allows Player To Call On SystemParser instance"""
        if (index != None):
            return self.system_container[index]
        else: return self.system_container

    def __len__(self):
        """Calculates All Files And Directories In System"""
        return len(self(0)) + len(self(1))

    def __enter__(self):
        """With Open Handler"""
        return self

    def __exit__(self, exc_type, exc_value, traceback):
        """With Open Handler"""
        return True

    def initialise(self, work_path):
        """Initializes Current SystemParser instance around passed path"""
        gfe = lambda X: X.split('.')[-1].upper() # Get File Extension
        self.system_container = ([work_path], list()) # (Directories, Files)
        
        if not(os.path.exists(work_path)):
            raise FileNotFoundError(work_path)

        self.root_path = work_path; self._system_parse()
        self.system_container[0].pop(0) # Remove Root Directory
        self.file_types = list(set([gfe(X) for X in self(1)]))

    def _system_parse(self, counter=0):
        """Nitty Gritty Of The Actual SystemParser Using Brute Force"""
        while True:
            for path in self.system_container[0]:
                for contents in [self.merge(path, X) for X in self.ls(path)]:
                    appension_index = 0 if os.path.isdir(contents) else 1
                    if not(contents in self.system_container[appension_index]):
                        self.system_container[appension_index].append(contents)
                        
            if not(len(self.system_container[0]) == counter):
                counter = len(self.system_container[0])
            else: break

    def get_files_of_type(self, *args):
        """Returns Files With a Given Desired Extension"""
        if not(isinstance(args, str)):
            args = [X.upper() for X in args]
        else: args = [args.upper()]
            
        gfe = lambda X: X.split('.')[-1].upper() # Get File Extension
        return [X for X in self(1) if gfe(X) in args]

    def get_system(self):
        """Returns All Files And Directories"""
        container = self(0)
        container.extend(self(1))
        return sorted(container)

    def search(self, key):
        """Searches Given System Using An Argument"""
        system = self.get_system()
        
        if (isinstance(key, str)):
            return [X for X in system if key in X.lower()]
        elif (callable(key)):
            return [X for X in system if key(X)]
        else:
            raise SystemParser.SystemParserExceptions.UnidentifiableKeyException(
                "Expected Function Or String Like Object, Not {0}".format(type(key)))
        
    @staticmethod
    def generate_system_traverser(path, index=(0, 1)):
        """Generates Entire System"""
        with System(path) as sys:
            if isinstance(index, tuple):
                array = sys(index[0])
                array.extend(sys(index[1]))
            elif isinstance(index, int):
                array = sys(index)
            else:
                raise SystemParser.SystemParserExceptions.UnidentifiableKeyException(
                    "Expected Tuple Or Integer Object, Not {0}".format(type(key)))

            for path in array: yield path
    
    ls = staticmethod(os.listdir)
    merge = staticmethod(os.path.join)
    path_to_title = staticmethod(lambda X: X.split('\\')[-1])
    truncate_path = lambda self, X: X[len(self.root_path)+1:]
    wrapper = PathWrapper()

    get_truncated_files = lambda self: [self.truncate_path(X) for X in self(1)]
    get_truncated_dirs = lambda self: [self.truncate_path(X) for X in self(0)]
    get_file_titles = lambda self: [self.path_to_title(X) for X in self(1)]
    get_file_types = lambda self: self.file_types
    get_files = lambda self: self(1)
    get_dirs = lambda self: self(0)
