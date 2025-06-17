import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'highlight'
})
export class HighlightPipe implements PipeTransform {
  transform(text: string, search: string|null): any {
    if (!search) return text;
    const pattern = new RegExp(`(${search})`, 'gi');
    return text.replace(pattern, `<i class="highlight">$1</i>`);
  }
}
