export const allocateSubDirectory = (inputValue :string,currentDirectory:string) : string => {
    const command = inputValue.replace(currentDirectory.replaceAll('\\\\','\\'),' ').replace('\\',' ')
    return command
}

export const replaceToOneSeparation = (currentDirectory: string) : string => {
    if(currentDirectory === '')
        return ''

    const command = (currentDirectory + '\\>').replaceAll('\\\\','\\').replaceAll('\\\\','\\')
    return command
}