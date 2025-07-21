import { Routes } from '@angular/router';
import { VideoList } from './video-list/video-list';
import { VideoUpload } from './video-upload/video-upload';
import { VideoPlay } from './video-play/video-play';

export const routes: Routes = [
    {path:"", component:VideoList},
    {path:"upload", component:VideoUpload},
    {path:"view/:name", component:VideoPlay}
];
