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
    if (subscription == null) {
        const pushSub = await swRegistration.pushManager.subscribe({ userVisibleOnly: true, applicationServerKey: serverKey })
    }
    else {
        const p256hKey = arrayBufferToBase64(subscription.getKey("p256dh"));
        const authKey = arrayBufferToBase64(subscription.getKey("auth"));
    }
}

export const getCurrentSubscriptionDetails = async (serverKey: string): Promise<NotificationSubscription> => {
    const swRegistration = await navigator.serviceWorker.register("../service-worker.js")
    const subscription = await swRegistration.pushManager.getSubscription()
    if (subscription == null) return null
    return {
        EndPoint: subscription.endpoint,
        P256hKey: arrayBufferToBase64(subscription.getKey("p256dh")),
        AuthKey: arrayBufferToBase64(subscription.getKey("auth"))
    }
}

export const unsubscribeFromNotifications = async (): Promise<boolean> => {
    const swRegistration = await navigator.serviceWorker.register("../service-worker.js")
    const subscription = await swRegistration.pushManager.getSubscription();
    if (subscription == null) return true;
    return await subscription.unsubscribe();
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