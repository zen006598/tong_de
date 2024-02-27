import pinyin from "pinyin";

export function pinyinConverter(unconvertedValue, style = 'tong') {
  return pinyin(unconvertedValue, { style: style }).join(' ');
}