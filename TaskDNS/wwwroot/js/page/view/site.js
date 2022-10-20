const fieldHistoryCommand = document.getElementById("listOutputCommand")
const fieldInputCommand = document.getElementById("inputCommand")

let onPressInputCommand = false
let countCurrentCommandHistory = 0
let countCurrentDirectories = 0

let mainDirectory = 'C:\\'
let DirectoriesArray = []

statusServerCommand = 2
command = ''
/**
   * JavaScript функция - отправка данных на сервер
    * @param {number} dataTime текущее время.
    * @param {number} inputFieldText текс из InputField.
    */
const addCommandAsync = async (dataTime, inputFieldText) => {

    urlAddOnServer = "https://localhost:7032/Server/add"
    dataClient = { id: 0, data: dataTime, textCommand: inputFieldText.replace('\"', '') }

    li = document.createElement('li')
    li.append(inputFieldText + '\n')

    fieldHistoryCommand.appendChild(li) 

    if (inputFieldText !== '') {
        await fetch(urlAddOnServer, {
            method: "POST",
            redirect: "follow",
            body: JSON.stringify(dataClient),
            headers: { 'Content-Type': 'application/json' }
        })
    }
}

/**
   * JavaScript функция - получающая путь у сервера.
   */
const getDirectoryAsync = async () => {

    urlGetDirectory = "https://localhost:7032/Server/getDirectory"
    responce = await fetch(urlGetDirectory)
    directory = await responce.text()

    mainDirectory = directory
    downScroll()
    fieldInputCommand.value = mainDirectory + '\>'
}

/**
   * JavaScript функция - обеспечивающая получения списка директорий относительна текущей(главной) директории.
   */
const getDirectoriesAsync = async () => {

     urlGetDirectories = "https://localhost:7032/Server/getDirectories"
     responce = await fetch(urlGetDirectories)
     directories = await responce.json()

     DirectoriesArray = directories
}

/**
   * JavaScript функция - обеспечивающая получение истории из базы данных.
   */
const getHistoryAsync = async () => {

    urlGetHistory = "https://localhost:7032/Server/getHistory"
    response = await fetch(urlGetHistory)
    dataHistory = await response.json()

    for (let i in dataHistory) {
        let li = document.createElement('li')
        li.append(dataHistory[i].textCommand + '\n')

        fieldHistoryCommand.appendChild(li)
        downScroll()
    }

    await getDirectoryAsync()
    await getDirectoriesAsync()
}

/**
   * JavaScript функция - обеспечивающая подключение к WebSoket.
   */
const connectToHubAsync = async () => {
    const hub = new signalR.HubConnectionBuilder() 
        .withUrl("/chat")
        .build()
    hubConnection = hub
    hub.on("Send", function (dataHub) {
        statusServerCommand = dataHub.status
        getDirectoriesAsync()
        writeHistoryCommand(dataHub.status, dataHub.output)
        getDirectoryAsync()
        downScroll()
    })

    fieldInputCommand.value = mainDirectory + '\>'
    hub.start()
}

/**
   * JavaScript функция - посылающая на сервер запрос на отмену команды.
   */
const stopCommandAsync = async () => {

    urlStopCommand = "https://localhost:7032/Server/Stop"

    await fetch(urlStopCommand, {
        method: "POST",
        redirect: "follow",
        body: JSON.stringify(),
        headers: { 'Content-Type': 'application/json' }
    })
}

/**
   * JavaScript функция - запускающая необходимые команды после загрузки.
   */
window.onload = async () => {
    await getHistoryAsync(fieldHistoryCommand)
    await connectToHubAsync()
    fieldInputCommand.onkeydown = async () => {
        if (statusServerCommand === 0) {
            if (event.key === 'Control') {
                await stopCommandAsync();
            }
            else {
                return false
            }
        }
        else
            checkKey(event.key)
    }
    fieldInputCommand.focus()
}

/**
   * JavaScript функция - обрабатывающая код нажатой клавиши для выполнения комманд.
   * @param {string} key является кодом нажатой кнопки.
   */
function checkKey(key) {
    
    if (onPressInputCommand === false) {
        countCurrentCommandHistory = dataHistory.length
        onPressInputCommand = true
    }

    if (key === 'Backspace' || key === 'ArrowLeft' || key === 'A') {
         lenghtInputCommand = fieldInputCommand.value.length - 1
        if (fieldInputCommand.value[lenghtInputCommand] === '>') {
            return key;
        }
    }

    if (key === 'ArrowUp') {
        countCurrentCommandHistory--
        if (countCurrentCommandHistory < 0)
            countCurrentCommandHistory = 0

        fieldInputCommand.value = mainDirectory + '\> ' + dataHistory[countCurrentCommandHistory].textCommand
    }
    else if (key === 'ArrowDown') {
        countCurrentCommandHistory++
        if (countCurrentCommandHistory > dataHistory.length - 1)
            countCurrentCommandHistory = dataHistory.length - 1

        fieldInputCommand.value = mainDirectory + '\> ' + dataHistory[countCurrentCommandHistory].textCommand
    }
    if (key === 'Enter') {

        fieldHistoryCommand.value += fieldInputCommand.value + '\n' + '\n'
        getCommand(fieldInputCommand.value)
        addCommandAsync('Data', command)
        fieldInputCommand.value = mainDirectory + '\> '

        command = ''
    }

    if (key === 'Tab') {
        if (DirectoriesArray != null) {
            childDirictory = currentDirectory(mainDirectory, DirectoriesArray[countCurrentDirectories])

            fieldInputCommand.value = mainDirectory + '\>' + childDirictory
            fieldInputCommand.focus()
        }
    }

    if (fieldInputCommand.value === '')
        fieldInputCommand.value = mainDirectory + '\>'
    
    return key
}
/**
   * JavaScript функция - прокручивающая страницу в самый низ
   */
function downScroll() {
    fieldInputCommand.scrollIntoView(false)
}

/**
   * JavaScript функция - запись комманд в historyCommand
    * @param {number} status состояние выполненной команды на сервере.
     * @param {string} dataText данные пришедшие от сервера.
   */
function writeHistoryCommand(status, dataText) {

    if (status != 2) {

        li = document.createElement('li')
        pre = document.createElement('pre')

        pre.append(dataText)
        li.append(pre)

        if (status == 1)
            li.classList.add('red')
        else
            li.classList.add('white')
        
        fieldHistoryCommand.appendChild(li)
        downScroll()
    }
}

/**
   * JavaScript функция - обработка веденной команды от клиента перед отправкой на сервер.
   */
function getCommand(inputCommandText) {
  
    let isStartReadCommand
    let isReadCommand

    for (let i = 0; i != inputCommandText.length;i++) {
        if (inputCommandText[i] === '>') {
            isStartReadCommand = true
            i++
        }
        if (isStartReadCommand) {
            if (inputCommandText[i] != ' ')
                isReadCommand = true
            if (isReadCommand)
                command += inputCommandText[i]
        }
    }
}
/**
   * JavaScript функция - сравнивающая имя главной директории(текущей) с под директорией выбранной клиентом.
   * Так же обраезает совпадающие имена выбранной директории с главной(корневой).
   */
function currentDirectory(mainDirectory,Directories) {
    readyDirectory = ''

    for (let i = 0; i < Directories.length ; i++) {
        if (Directories[i] != mainDirectory[i])
            readyDirectory += Directories[i]
    }

    if (countCurrentDirectories === DirectoriesArray.length - 1) 
        countCurrentDirectories = 0
    else
        countCurrentDirectories++

    readyDirectory = readyDirectory.slice(0)
    return readyDirectory
}
