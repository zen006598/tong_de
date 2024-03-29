export default {
    appType: 'custom',
    root: 'Assets',
    build: {
        manifest: true,
        outDir: '../dist',
        emptyOutDir: true,
        assetsDir: '',
        rollupOptions: {
            input: {
                main: 'Assets/main.js',
                itemCreate: 'Assets/itemCreate.js',
                itemEdit: 'Assets/itemEdit.js',
                izitoast: 'Assets/izitoast.js',
                items: 'Assets/items.js',
                itemsFilter: 'Assets/itemsFilter.js',
                errorAlert: 'Assets/errorAlert.js',
            },
            output: {
                entryFileNames: '[name].js',
                chunkFileNames: '[name].js',
                assetFileNames: '[name].[ext]'
            }
        },
    },
    server: {
        port: 5173,
        strictPort: true,
        hmr: {
            clientPort: 5173
        }
    }
}
