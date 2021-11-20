#!/usr/bin/env node
const fs = require('fs')
const child_process = require('child_process')

const files = fs.readdirSync('.')

for (const file of files) {
    if (!file.endsWith('Tile.png')) {
        continue;
    }

    const [basename] = file.split('.png')

    const out_name = `${basename}_Small.png`

    child_process.execSync(`convert ${file}'[256x256]' ${out_name}`)
}

