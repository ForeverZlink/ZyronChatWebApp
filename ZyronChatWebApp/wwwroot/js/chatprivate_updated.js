"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessagePrivate", function (message) {
    var date = new Date();
    
    var div = document.createElement("div");
    div.classList.add("alert", "alert-secondary", "bg-dark");

    var MessageHasArrived = document.createElement("h4");
    MessageHasArrived.innerText = "New message has arrived!";
    MessageHasArrived.classList.add("text-success");

    var p = document.createElement("p");
    p.classList.add("text-start", "text-light", "fs-4");

    var br = document.createElement("br")
    
    p.innerText = message
    p.append(br)
    p.append(date.toLocaleString())
    
    div.append(MessageHasArrived)
    div.append(p)

    document.getElementById("NewMessagesNow").prepend(div);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you
    // should be aware of possible script injection concerns.
    
    

    
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessagePrivate", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});