﻿const fieldHistoryCommand = document.getElementById("listOutputCommand")
const fieldInputCommand = document.getElementById("inputCommand")

let onPressInputCommand = false
let countCurrentCommandHistory = 0
let countCurrentDirectories = 0

let mainDirectory = 'C:\\'
let directoriesArray = []
let historyArray = []

let statusServerCommand = 2
let command = ''

/**
    * Отправка данных на сервер.
    * @param {number} dataTime текущее время.
    * @param {number} fieldInputValue текс из InputField.
    */
const addCommandAsync = async (dataTime, fieldInputValue) => {

    const urlAddOnServer = "https://localhost:7032/command/add"
    const dataClient = { id: 0, data: dataTime, textCommand: fieldInputValue.replace('\"', '') }

    const li = document.createElement('li')
    li.append(fieldInputValue + '\n')

    fieldHistoryCommand.appendChild(li) 

    if (fieldInputValue !== '') {
        await fetch(urlAddOnServer, {
            method: "POST",
            redirect: "follow",
            body: JSON.stringify(dataClient),
            headers: { 'Content-Type': 'application/json' }
        })
    }
}
/**
   * Получение пути от сервера.
   */
const getDirectoryAsync = async () => {

    const urlGetDirectory = "https://localhost:7032/command/getDirectory"
    const responce = await fetch(urlGetDirectory)
    const directory = await responce.text()

    mainDirectory = directory
    downScroll()
    fieldInputCommand.value = mainDirectory + '\>'
}
/**
   * Получение списка директорий относительна текущей(главной) директории.
   */
const getDirectoriesAsync = async () => {

    const urlGetDirectories = "https://localhost:7032/command/getDirectories"
     const responce = await fetch(urlGetDirectories)
     const directories = await responce.json()

     directoriesArray = directories
}
/**
   * Получение истории из базы данных.
   */
const getHistoryAsync = async () => {

    const urlGetHistory = "https://localhost:7032/command/getHistory"
    const response = await fetch(urlGetHistory)
    const dataHistory = await response.json()

    historyArray = dataHistory

    for (let i in historyArray) {
        const li = document.createElement('li')
        li.append(historyArray[i].textCommand + '\n')

        fieldHistoryCommand.appendChild(li)
        downScroll()
    }

    await getDirectoryAsync()
    await getDirectoriesAsync()
}
/**
   * Подключение к WebSoket.
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
   * Отправка POST-запроса на сервер для отмены команды.
   */
const stopCommandAsync = async () => {

    const urlStopCommand = "https://localhost:7032/command/Stop"

    await fetch(urlStopCommand, {
        method: "POST",
        redirect: "follow",
        body: JSON.stringify(),
        headers: { 'Content-Type': 'application/json' }
    })
}
/**
   * Запуск необходимых команды после загрузки.
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
        else {
            checkKey(event.key)
        }
    }
    fieldInputCommand.focus()
}
/**
   * Проверка нажатой клавишы для выполнения определенной логики.
   * @param {string} key является кодом нажатой кнопки.
   */
function checkKey(key) {
    
    if (onPressInputCommand === false) {
        countCurrentCommandHistory = historyArray.length
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
        if (countCurrentCommandHistory < 0) {
            countCurrentCommandHistory = 0
        }
        fieldInputCommand.value = mainDirectory + '\> ' + historyArray[countCurrentCommandHistory].textCommand
    }
    else if (key === 'ArrowDown') {
        countCurrentCommandHistory++
        if (countCurrentCommandHistory > historyArray.length - 1) {
            countCurrentCommandHistory = historyArray.length - 1
        }

        fieldInputCommand.value = mainDirectory + '\> ' + historyArray[countCurrentCommandHistory].textCommand
    }
    if (key === 'Enter') {

        fieldHistoryCommand.value += fieldInputCommand.value + '\n' + '\n'
        getCommand(fieldInputCommand.value)
        addCommandAsync('Data', command)
        fieldInputCommand.value = mainDirectory + '\> '

        command = ''
    }

    if (key === 'Tab') {
        if (directoriesArray != null) {
            childDirictory = currentDirectory(mainDirectory, directoriesArray[countCurrentDirectories])

            fieldInputCommand.value = mainDirectory + '\>' + childDirictory
            fieldInputCommand.focus()
        }
    }

    if (fieldInputCommand.value === '') {
        fieldInputCommand.value = mainDirectory + '\>'
    }

    return key
}
/**
   * Прокручивание страницы в самый низ.
   */
function downScroll() {
    fieldInputCommand.scrollIntoView(false)
}
/**
   * Запись комманд в historyCommand.
   * @param {number} status состояние выполненной команды на сервере.
   * @param {string} dataText данные пришедшие от сервера.
   */
function writeHistoryCommand(status, dataText) {

    if (status != 2) {

        const li = document.createElement('li')
        const pre = document.createElement('pre')

        pre.append(dataText)
        li.append(pre)

        if (status == 1) {
            li.classList.add('red')
        }
        else {
            li.classList.add('white')
        }

        fieldHistoryCommand.appendChild(li)
        downScroll()
    }
}
/**
   * Обработка веденной команды от клиента перед отправкой на сервер.
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
            if (inputCommandText[i] != ' ') {
                isReadCommand = true
            }
            if (isReadCommand) {
                command += inputCommandText[i]
            }
        }
    }
}
/**
   * Сравнение имени главной директории(текущей) с под директорией выбранной клиентом.
   * Так же обраезает совпадающие имена выбранной директории с главной(корневой).
   */
function currentDirectory(mainDirectory,Directories) {
    let readyDirectory = ''

    for (let i = 0; i < Directories.length ; i++) {
        if (Directories[i] != mainDirectory[i]) {
            readyDirectory += Directories[i]
        }
    }

    if (countCurrentDirectories === directoriesArray.length - 1) {
        countCurrentDirectories = 0
    }
    else {
        countCurrentDirectories++
    }

    readyDirectory = readyDirectory.slice(0)
    return readyDirectory
}
