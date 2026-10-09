const loadedScripts = new Map<string, Promise<void>>();

export const loadScript = (src: string): Promise<void> => {
    if (loadedScripts.has(src)) return loadedScripts.get(src)!;

    const promise = new Promise<void>((resolve, reject) => {
        const existing = document.querySelector(`script[src="${src}"]`);
        if (existing) {
            resolve();
            return;
        }

        const script = document.createElement('script');
        script.src = src;
        script.async = true;
        script.defer = true;
        script.onload = () => resolve();
        script.onerror = () => {
            loadedScripts.delete(src);
            reject(new Error(`Failed to load script: ${src}`));
        };

        document.head.appendChild(script);
    });

    loadedScripts.set(src, promise);
    return promise;
};