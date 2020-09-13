# >> InputParser Class Definition Version\\Rendition 1.00  || (C) M.K. 2017 << #

from re import findall, escape, split # Regex Imports for line\string parsing

class IPErrors(object):
    """Exception Class Holder"""
    class InputFormatError(Exception):
        """Thrown when input string isn't a string"""
        def __init__(self, type):
            string = "Input Of Type '{}' Not Accepted".format(type)
            super().__init__(string)
    
    class ContainerTypeError(Exception):
        """Thrown when container isn't a tuple"""
        def __init__(self, string, type):
            string = "'{}' Must Be A Tuple Not A '{}'".format(string, type)
            super().__init__(string)

    class UnacceptedTypeInContainerError(Exception):
        """Thrown when value in container is of undesired type"""
        def __init__(self, index, string, type):
            string = "'{}' Has An Unaccepted Value of Type '{}' At Index '{}'".format(
                string, type, index
            )
            
            super().__init__(string)


class Flag(object):
    """Container class for flag and any corresponding
    arguments flag was passed with during initialisation"""
    def __init__(self, flag, *args):
        self.flag, self.args = flag, args

    def __repr__(self):
        if isinstance(self.flag, str):
            return self.flag

        try:
            return str(self.flag)
        except:
            return "Flag Print Error"

    def __eq__(self, other):
        f_check = str(self.flag) == str(other.flag)
        a_check = self.args == other.args
            
        if isinstance(other, Flag):
            # comparing everything you can
            return f_check and a_check
        elif isinstance(other, str):
            return f_check
        elif isinstance(other, list):
            return a_check
        else:
            raise TypeError("Other Type Not Accepted : {} :".format(type(other)))

    def __len__(self):
        return len(self.args)


class InputParser(object):
    """Class to parse user input. Quite self explanatory :)\n
    c_chars are the container characters used like \" or \'.
    f_chars are the characters used to interpret flags like '-' or '+'.\n\n
    Warning: Avoid using multiple c_chars in the same string like "'hello' there" """
    def __init__(self, c_chars=('"', "'"), f_chars=('-',)):
        self.container_chars, self.flag_chars = c_chars, f_chars

    def _exception_check(self, s_input):
        """Wether any value types are wrong and then throws an exception"""
        if not(isinstance(s_input, str)):
            raise IPErrors.InputFormatError(type(s_input))
        elif not(isinstance(self.container_chars, tuple)):
            raise IPErrors.ContainerTypeError(
                "Container Chars", type(self.container_chars))
        elif not(isinstance(self.flag_chars, tuple)):
            raise IPErrors.ContainerTypeError(
                "Flag Chars", type(self.container_chars))
        elif False in [isinstance(X, str) for X in self.container_chars]:
            index = [isinstance(X, str) for X in self.container_chars].index(False)
            
            raise IPErrors.UnacceptedTypeInContainerError(
                index, "Container Chars", type(self.container_chars[index])
            )
        elif False in [isinstance(X, str) for X in self.flag_chars]:
            index = [isinstance(X, str) for X in self.flag_chars].index(False)
            
            raise IPErrors.UnacceptedTypeInContainerError(
                index, "Flag Chars", type(self.container_chars[index])
            )

    def _get_str_contained_terms(self, sub_string):
        # extracts items within container chars
        container_array = list() # empty list to contain results
        get_search_term = gst = lambda X: '{0}([^{0}]*){0}'.format(X)
         
        for C in self.container_chars:
            container_array.extend(
                ["{}{}{}".format(C, X, C) for X in findall(gst(C), sub_string)]
			)

        return container_array

    def parse_input(self, s_input):
        """parses input from string.
        Returns Tuple containing Flags"""
        self._exception_check(s_input)

        if len(self.flag_chars) == 0:
            # no potential flags in parser
            for C in self.container_chars:
                # remove container characters
                s_input = s_input.replace(C, '')
                
            return s_input.split(' ')

        s_input, found = self.flag_chars[0] + '\0 ' + s_input, list()
        contained = self._get_str_contained_terms(s_input)
        c_index = contained_index = 0

        for c_arg in contained:
            #if not(c_arg in s_input): continue
            s_input = s_input.replace(c_arg, '\0')

        for flagsplit in split('|'.join(map(escape, self.flag_chars)), s_input):
            if len(flagsplit) < 1: continue # ignore 0 length
            flag, container = flagsplit.split(' ')[0], list()
            flag = flag if flag != '\0' else None 
            
            for arg in ' '.join(flagsplit.split(' ')[1:]).split(' '):
                if len(arg) == 0:
                    continue
                elif arg == '\0': # contained_item
                    container.append(contained[c_index][1:-1])
                    # 1 to -1 removes container chars from str
                    c_index = c_index + 1 # increment by one
                else:
                    container.append(arg)

            found.append((flag, container)) # Newer version returns tuple for c#.
            #found.append(Flag(flag, *container)) # Original Rendition Used Flags.
            
        return tuple(found)

    @staticmethod
    def _input_parse(string, c_chars=None, f_chars=None):
        """Unrecommended approach to parse input statically"""
        ip = InputParser()

        if not(c_chars is None):
            ip.container_chars = c_chars
        if not(f_chars is None):
            ip.flag_chars = f_chars

        return ip.parse_input(string)
