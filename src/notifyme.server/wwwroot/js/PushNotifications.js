var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
export const checkAndRequestNotificationPermission = () => __awaiter(void 0, void 0, void 0, function* () {
    if (Notification.permission !== "granted") {
        const hasPermission = yield Notification.requestPermission();
        return hasPermission === "granted";
    }
    return true;
});
export const registerServiceWorker = (serverKey) => __awaiter(void 0, void 0, void 0, function* () {
    const swRegistration = yield navigator.serviceWorker.register("../service-worker.js");
    const subscription = yield swRegistration.pushManager.getSubscription();
    console.log(subscription);
    if (subscription == null) {
        const pushSub = yield swRegistration.pushManager.subscribe({ userVisibleOnly: true, applicationServerKey: serverKey });
        console.log(`New subscription added: ${pushSub.toJSON()}`);
    }
    else {
        console.log("push subscription is not null, therefore must already exist");
        const p256hKey = arrayBufferToBase64(subscription.getKey("p256dh"));
        const authKey = arrayBufferToBase64(subscription.getKey("auth"));
        console.log(p256hKey);
        console.log(authKey);
    }
});
export const getCurrentSubscriptionDetails = (serverKey) => __awaiter(void 0, void 0, void 0, function* () {
    const swRegistration = yield navigator.serviceWorker.register("../service-worker.js");
    const subscription = yield swRegistration.pushManager.getSubscription();
    console.log(subscription);
    if (subscription == null)
        return null;
    return {
        EndPoint: subscription.endpoint,
        P256hKey: arrayBufferToBase64(subscription.getKey("p256dh")),
        AuthKey: arrayBufferToBase64(subscription.getKey("auth"))
    };
});
function arrayBufferToBase64(buffer) {
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}
//# sourceMappingURL=PushNotifications.js.map