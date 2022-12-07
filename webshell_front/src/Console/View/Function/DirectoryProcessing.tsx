/*
 *  Функция удаляющая лишние символы разделения для под папок.
 */
export const allocateSubDirectory = (inputValue :string,currentDirectory:string) : string => {
    const subDirectory = inputValue.replace(currentDirectory.replaceAll('\\\\','\\'),' ').replace('\\',' ')
    return subDirectory
}
/*
 *  Функция заменяющие несколько символов разделения на один символ.
 */
export const replaceToOneSeparation = (currentDirectory: string) : string => {
    if(currentDirectory === '')
        return ''

    const directory = (currentDirectory + '\\>').replaceAll('\\\\','\\').replaceAll('\\\\','\\')
    return directory
}