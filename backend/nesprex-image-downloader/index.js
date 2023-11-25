const axios = require('axios');
const fs = require('fs');
const path = require('path');

const { Kafka } = require('kafkajs')

const start = async () => {
    const kafka = new Kafka({
        clientId: 'nesprex-image-downloader',
        brokers: ['localhost:9092'],
    })

    const consumer = kafka.consumer({ groupId: 'nesprex-image-downloader_consumer' })

    await consumer.connect()

    await consumer.subscribe({ topic: "nesprex.images", fromBeginning: true })

    await consumer.run({
        eachMessage: async ({ topic, partition, message }) => {
            var mappedMessage = JSON.parse(message.value.toString());

            await downloadImage(mappedMessage);
        },
    })
}

start().catch(console.error)

const downloadImage = async ({ Folders, FileName, ImageUrl }) => {

    let downloadDir = path.join(__dirname, 'images');

    const extraPathsUnderscored = Folders.map(pathPart => pathPart.replace(/ /g, '_'));

    downloadDir = path.join(downloadDir, ...extraPathsUnderscored);

    if (!fs.existsSync(downloadDir)) {
        fs.mkdirSync(downloadDir, { recursive: true });
    }

    const fileNameWithUnderscores = FileName.replace(/ /g, '_');

    const fileExtension = path.extname(ImageUrl);

    const filePathToSave = path.join(downloadDir, fileNameWithUnderscores + fileExtension);

    console.log(`[DOWNLOADING] ${filePathToSave}`);

    try {
        const response = await axios({
            method: 'get',
            url: ImageUrl,
            responseType: 'stream',
        });

        response.data.pipe(fs.createWriteStream(filePathToSave));

        response.data.on('finish', () => {
            console.log(`[DOWNLOADED] ${filePathToSave}`);
        });

        response.data.on('error', (error) => {
            console.log(`Error downloading image to ${filePathToSave}: ${error}`);
        });
    } catch (error) {
        console.log(`Error downloading image to ${filePathToSave}: ${error.message}`);
    }
}