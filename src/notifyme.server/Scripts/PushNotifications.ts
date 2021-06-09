type NotificationSubscription = {
    EndPoint: string,
    P256hKey: string,
    AuthKey: string
}

export const checkAndRequestNotificationPermission = async (): Promise<Boolean> => {
    if (Notification.permission !== "granted") {
        const hasPermission = await Notification.requestPermission()
        return hasPermission === "granted"
    }
    return true
}

export const registerServiceWorker = async (serverKey: string) => {
    const swRegistration = await navigator.serviceWorker.register("../service-worker.js")
    const subscription = await swRegistration.pushManager.getSubscription()
    console.log(subscription)
    if (subscription == null) {
        const pushSub = await swRegistration.pushManager.subscribe({ userVisibleOnly: true, applicationServerKey: serverKey })
        console.log(`New subscription added: ${pushSub.toJSON()}`)
    }
    else {
        console.log("push subscription is not null, therefore must already exist")
        const p256hKey = arrayBufferToBase64(subscription.getKey("p256dh"));
        const authKey = arrayBufferToBase64(subscription.getKey("auth"));
        console.log(p256hKey);
        console.log(authKey);
    }
}

export const getCurrentSubscriptionDetails = async (serverKey: string): Promise<NotificationSubscription> => {
    const swRegistration = await navigator.serviceWorker.register("../service-worker.js")
    const subscription = await swRegistration.pushManager.getSubscription()
    console.log(subscription);
    if (subscription == null) return null
    return {
        EndPoint: subscription.endpoint,
        P256hKey: arrayBufferToBase64(subscription.getKey("p256dh")),
        AuthKey: arrayBufferToBase64(subscription.getKey("auth"))
    }
}

function arrayBufferToBase64(buffer) {
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}