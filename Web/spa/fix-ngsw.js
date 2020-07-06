var replace = require('replace-in-file');

const options = {
    files: [
        'dist/spa/ngsw-worker.js',
    ],
    from: /ngsw:db/g,
    to: "ngsw:db-sss",
    allowEmptyPaths: false,
};
try {
    let changedFiles = replace.sync(options);
    if (changedFiles == 0) {
        throw "Please make sure that the file '" + options.files + "' has ngsw:db";
    }
    console.log('replace ngsw:db to ngsw:db-sss');
} catch (error) {
    console.error('Error occurred:', error);
    throw error
}