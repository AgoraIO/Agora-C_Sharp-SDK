import { execSync } from "child_process";
import * as fs from "fs";
import path from "path";

let AllDirPath = [
    "../../Agora-C_Sharp-RTC-SDK/Code/Rtc",
    "../../Agora-C_Sharp-RTC-SDK/Code/Rtc/Types",
];


function DeleteOldDocWithFile(filePath: string) {
    let context = fs.readFileSync(filePath, { encoding: 'utf-8' });
    let readLines = context.split('\n');
    let writeLines = [];
    for (let line of readLines) {
        let lineTrim = line.trim();
        if (lineTrim.startsWith('///') ||
            lineTrim.startsWith('//') ||
            lineTrim.startsWith('/* ') ||
            lineTrim.startsWith('/**') ||
            lineTrim.startsWith('*') ||
            lineTrim.endsWith('*/')) {
            continue;
        }
        writeLines.push(line);
    }
    fs.writeFileSync(filePath, writeLines.join('\n'));
}


export function DeleteAllOldDoc() {
    for (let dirPath of AllDirPath) {
        dirPath = path.join(process.cwd(), dirPath);
        let fileList = fs.readdirSync(dirPath);
        for (let file of fileList) {
            file = path.join(dirPath, file);
            let state = fs.statSync(file);
            if (state.isFile()) {
                DeleteOldDocWithFile(file);
            }
        }
    }
}

enum ContentType {
    None,
    //class as parameter
    Clazz,
    Enumz,
    //real interface, IRtcEngine, IRtcEngineEx like
    Interfacez,
    //IAudioFrameObserver like
    Callbackz
};

interface Content {
    type: ContentType,
    name: string,
    braces: string[]
};


function AddDocWithFile(filePath: string) {
    let content: Content = {
        type: ContentType.None,
        name: "",
        braces: []
    };
    let str = fs.readFileSync(filePath, { encoding: 'utf-8' });
    let lines = str.split('\n');
    let lineLength = lines.length;
    let i = 0;
    while (i < lineLength) {
        let line = lines[i];
        line = line.trim();
        if (content.type == ContentType.None) {
            let matchEnumArray = line.match(/^public enum ([a-zA-Z0-9_]+)/)
            if (matchEnumArray != null) {
                content.type = ContentType.Enumz;
                content.name = matchEnumArray[1].toLowerCase().replaceAll('_', '');
                lines.splice(i, 0, `/* enum_${content.name} */`);
                i = i + 2;
                lineLength++;
                continue;
            }
            let matchClassArray = line.match(/^public class ([a-zA-Z0-9_]+)/)
            if (matchClassArray != null) {
                content.type = ContentType.Clazz;
                content.name = matchClassArray[1].toLowerCase();
                lines.splice(i, 0, `/* class_${content.name} */`);
                i = i + 2;
                lineLength++;
                continue;
            }
        }
        else if (content.type == ContentType.Enumz) {
            if (line.endsWith(',')) {
                let enumParamName: string;
                if (line.includes('=')) {
                    enumParamName = line.substring(0, line.indexOf('='));
                }
                else {
                    enumParamName = line.substring(0, line.indexOf(','))
                }
                enumParamName = enumParamName.trim();
                lines.splice(i, 0, `/* enum_${content.name}_${enumParamName} */`)
                i = i + 2;
                lineLength++;
                continue;
            }
        }
        else if (content.type == ContentType.Clazz) {
            let matchParamsArray = line.match(/^public ([a-zA-Z0-9_<>]+) ([a-zA-Z0-9_]+)(;| =)/)
            if (matchParamsArray != null) {
                lines.splice(i, 0, `/* class_${content.name}_${matchParamsArray[2]} */`)
                i = i + 2;
                lineLength++;
                continue;
            }
        }

        if (content.type != ContentType.None) {
            if (line.includes('{')) {
                content.braces.push('{')
            }
            else if (line.includes('}')) {
                content.braces.pop();
                if (content.braces.length == 0) {
                    content.type = ContentType.None;
                    content.name = "";
                }
            }
        }
        i++;
    }

    fs.writeFileSync(filePath, lines.join('\n'), { encoding: 'utf-8' });
    execSync(`clang-format -i ${filePath}`);
}


export function AddAllDoc() {
    for (let dirPath of AllDirPath) {
        dirPath = path.join(process.cwd(), dirPath);
        let fileList = fs.readdirSync(dirPath);
        for (let file of fileList) {
            file = path.join(dirPath, file);
            let state = fs.statSync(file);
            if (state.isFile()) {
                AddDocWithFile(file);
            }
        }
    }

}





