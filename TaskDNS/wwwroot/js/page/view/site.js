
const historyCommand = document.getElementById("listOutputCommand")
const inputCommand = document.getElementById("inputCommand")

let onPressInputCommand = false
let countPress = 0
let countCurrentDirectories = 0

let mainDirectory = "C:\\"
let DirectoryesArray
let command = ''

/**
   * JavaScript функция - отправка данных на сервер
    * @param {number} dataTime текущее время.
    * @param {number} inputFieldText текс из InputField.
    */
const addCommandAsync = async (dataTime, inputFieldText) => {

    urlAddOnServer = "https://localhost:7032/Server/add"
    dataClient = { id: 0, data: dataTime, textCommand: inputFieldText.replace('\"', '') }

    li = document.createElement('li')
    li.append(inputFieldText + "\n")

    historyCommand.appendChild(li) 

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
    inputCommand.value = mainDirectory + "\>"
}

/**
   * JavaScript функция - обеспечивающая получения списка директорий относительна текущей(главной) директории.
   */
const getDirectoriesAsync = async () => {

     urlGetDirectories = "https://localhost:7032/Server/getDirectories"
     responce = await fetch(urlGetDirectories)
     directories = await responce.json()

     DirectoryesArray = Object.keys(directories).map(key => [directories[key]])
}

/**
   * JavaScript функция - обеспечивающая получение истории из базы данных.
   */
const getHistoryAsync = async () => {

    urlGetHistory = "https://localhost:7032/Server/getHistory"
    response = await fetch(urlGetHistory)
    dataHistory = await response.json()

    for (let i in dataHistory) {
        let li = document.createElement("li")
        li.append(dataHistory[i].textCommand + "\n")

        historyCommand.appendChild(li)
        downScroll()
    }

    await getDirectoryAsync()
    await getDirectoriesAsync()
}

/**
   * JavaScript функция - обеспечивающая подключение к WebSoket.
   */
const connectToHub = async () => {
    const hub = new signalR.HubConnectionBuilder() 
        .withUrl("/chat")
        .build()
    hubConnection = hub
    hub.on("Send", function (data) {
        getDirectoriesAsync()
        writeHistoryCommand(data.status, data.output)
        getDirectoryAsync()
        downScroll()

        if (data.status === 0)
            offInput()
        else
            onInput()
    })

    inputCommand.value = mainDirectory + "\>"
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
window.onload = () => {
    getHistoryAsync(historyCommand)
    connectToHub()
    inputCommand.onkeydown = () => { return checkKey(event.key) }
    inputCommand.focus()
}

/**
   * JavaScript функция - обрабатывающая код нажатой клавиши для выполнения комманд.
    * @param {string} key является кодом нажатой кнопки.
   */
function checkKey(key) {
    
    if (onPressInputCommand === false) {
        countPress = dataHistory.length
        onPressInputCommand = true
    }

    if (key === "Backspace" || key === "ArrowLeft" || key === "A") {
         lenghtInputCommand = inputCommand.value.length - 1
        if (inputCommand.value[lenghtInputCommand] == ">")
            return false
    }

    if (key === "ArrowUp") {
        countPress--
        if (countPress < 0)
            countPress = 0

        inputCommand.value = mainDirectory + "\> " + dataHistory[countPress].textCommand
    }
    else if (key === "ArrowDown") {
        countPress++
        if (countPress > dataHistory.length - 1)
            countPress = dataHistory.length - 1

        inputCommand.value = mainDirectory + "\> " + dataHistory[countPress].textCommand
    }
    if (key === "Enter") {

        historyCommand.value += inputCommand.value + "\n" + "\n"
        getCommand(inputCommand.value)
        addCommandAsync("Data", command)
        inputCommand.value = mainDirectory + "\> "

        command = ''
    }

    if (key === "Control") { //Остановка команды
        stopCommandAsync()
    }

    if (key === "Tab") {
        if (DirectoryesArray != null) {
            childDirictory = currentDirectory(mainDirectory, DirectoryesArray[countCurrentDirectories])

            inputCommand.value = mainDirectory + "\>" + childDirictory
            inputCommand.focus()
        }
    }
    
    if (inputCommand.value === '')
        inputCommand.value = mainDirectory + "\>"
    
    return key
}
/**
   * JavaScript функция - прокручивающая страницу в самый низ
   */
function downScroll() {
    inputCommand.scrollIntoView(false)
}
/**
   * JavaScript функция - имеет аннотацию с указанием типа
   */
function offInput() {
    inputCommand.onkeypress = () => { return false }
}
/**
   * JavaScript функция - имеет аннотацию с указанием типа
   */
function onInput() {
    inputCommand.onkeypress = () => { return true }
}
/**
   * JavaScript функция - запись комманд в historyCommand
    * @param {number} status состояние выполненной команды на сервере.
     * @param {string} dataText данные пришедшие от сервера.
   */
function writeHistoryCommand(status, dataText) {

    if (status != 2) {

        li = document.createElement("li")
        pre = document.createElement("pre")

        pre.append(dataText)
        li.append(pre)

        if (status == 1)
            li.classList.add("red")
        else
            li.classList.add("white")
        
        historyCommand.appendChild(li)
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
        if (inputCommandText[i] === ">") {
            isStartReadCommand = true
            i++
        }
        if (isStartReadCommand) {
            if (inputCommandText[i] != " ")
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
     readyDirectory = '';

    let array = Directories[0]
    for (let i = 0; i < array.length ; i++) {
        if (array[i] != mainDirectory[i])
            readyDirectory += array[i]
    }

    if (countCurrentDirectories === DirectoryesArray.length - 1) 
        countCurrentDirectories = 0
    else
        countCurrentDirectories++

    readyDirectory = readyDirectory.slice(0)
    return readyDirectory
}
