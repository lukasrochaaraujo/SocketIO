const Socket = new WebSocket("ws://localhost:5000/ws");

(function connect() {
    Socket.onopen = (e) => {
        console.log("connection estabished: " + e);
    };
    Socket.onclose = (e) => {
        console.log("connection closed: " + e);
    };
    Socket.onmessage = (e) => {
        append(list, readPackageMessage(e.data));
        console.log(e.data);
    };
})();

let list = document.getElementById("messages");
let button = document.getElementById("sendMessage");
button.addEventListener("click", () => Socket.send(createPackage()));

function append(list, message) {
    let li = document.createElement('li');
    li.appendChild(document.createTextNode(message));
    list.appendChild(li);
}

function createPackage() {
    let message = document.getElementById('command').value;
    let socketTargetId = document.getElementById('socketId').value;

    return `originteste@target${socketTargetId}@message${message}`;
}

function readPackageMessage(package) {
    let origin = package.split('@')[0].replace('origin', '');
    let target = package.split('@')[1].replace('target', '');
    let message = package.split('@')[2].replace('message', '');

    return 'from ' + origin + ' to ' + target + ': ' + message;
}