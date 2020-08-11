const converter = require('json-2-csv');

module.exports = async function (context, myBlob) {
    context.log("JavaScript blob trigger function processed blob \n Blob:", context.bindingData.blobTrigger, "\n Blob Size:", myBlob.length, "Bytes");
    var json = JSON.parse(myBlob.toString());
    var users = Object.keys(json).map(key => json[key]);
    var csv = await converter.json2csvAsync(users);
    
    context.log(csv);
    context.bindings.outputBlob = csv;
    context.done();
};