import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../../../core/services/api.service';

@Component({
  selector: 'app-upload-video',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `<div class="upload-container">
  <div class="upload-header">
    <h2>Upload Video</h2>
    <button class="close-btn" (click)="closeUpload()" type="button">×</button>
  </div>
  
    <form (ngSubmit)="upload()" #uploadForm="ngForm">
      <div class="form-left-right-block">
      
        <div class="form-left-block">
          <div class="form-group">
            <label>Title *</label>
            <input 
              type="text" 
              [(ngModel)]="title" 
              name="title"
              required
              maxlength="100"
              placeholder="Enter video title"
            />
          </div>

          <div class="form-group">
            <label>Tags</label>
            <div class="tag-input">
              <input 
                type="text" 
                [(ngModel)]="tagInput" 
                name="tagInput"
                (keyup.enter)="addTag()" 
                placeholder="Press Enter to add tag" 
              />
              <div class="tags-container">
                <div class="tag" *ngFor="let tag of tags; let i = index">
                  {{ tag }}
                  <span (click)="removeTag(i)" class="remove-tag">×</span>
                </div>
              </div>
            </div>
          </div>

        </div>
        <div class="form-right-block">
          <div class="form-group-upload-block">
            <div class="form-group">
              <label>Thumbnail *</label>
              <input 
                type="file" 
                (change)="onThumbnailSelected($event)" 
                accept="image/*"
                required
              />
              <small>Max size: 5MB</small>
            </div>

            <div class="form-group">
              <label>Video File *</label>
              <input 
                type="file" 
                (change)="onVideoSelected($event)" 
                accept="video/*"
                required
              />
              <small>Max size: 1GB</small>
            </div>
          </div>


          <div class="form-group">
            <label>Description</label>
            <textarea 
              [(ngModel)]="description"
              name="description"
              maxlength="500"
              placeholder="Describe your video"
            ></textarea>
          </div>
          
        </div>
      </div>
          <div class="message success" *ngIf="successMessage">
    <span>✓</span> {{ successMessage }}
  </div>
  <div class="message error" *ngIf="errorMessage">
    <span>⚠</span> {{ errorMessage }}
  </div>
      <div class="form-actions">
        <button 
          type="button" 
          class="cancel-btn" 
          (click)="closeUpload()"
          [disabled]="uploading"
        >
          Cancel
        </button>
        <button 
          type="submit" 
          class="upload-btn"
          [disabled]="uploading || !uploadForm.form.valid"
        >
          {{ uploading ? 'Uploading...' : 'Upload Video' }}
        </button>
      </div>
  </form>
</div>
`,
  styles: [`
.upload-container {
  width: 100%;
  height: 100%;
  margin: 0 auto;
  backdrop-filter: blur(20px);
  overflow: hidden;
}

.upload-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.upload-header h2 {
  margin: 0;
  font-size: 24px;
  font-weight: 700;
  background: linear-gradient(135deg, #ffffff 0%, #e0e0e0 100%);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.close-btn {
  background: rgba(255, 255, 255, 0.1);
  border: 1px solid rgba(255, 255, 255, 0.2);
  color: white;
  width: 32px;
  height: 32px;
  border-radius: 50%;
  cursor: pointer;
  font-size: 18px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
}

.close-btn:hover {
  background: rgba(255, 255, 255, 0.2);
  transform: scale(1.1);
}

form {
  padding: 0 24px 24px 24px;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  font-weight: 600;
  margin-bottom: 8px;
  color: rgba(255, 255, 255, 0.9);
  font-size: 14px;
}

.form-left-right-block {
  display: flex;
  justify-content: space-around;
  gap: 24px;
  height: 350px;
}
.form-left-block,
.form-right-block {
  flex: 1;
}

.form-group-upload-block {
  display: flex;
  gap: 16px;
}
.form-group-upload-block .form-group {
  flex: 1;
}

input[type="text"],
textarea,
input[type="file"] {
  width: 100%;
  padding: 12px 16px;
  border: none; 
  border-bottom: 1px solid rgba(255, 255, 255, 0.2);
  background: rgba(255, 255, 255, 0.05);
  color: white;
  font-size: 14px;
  transition: all 0.3s ease;
  box-sizing: border-box;
}

input[type="text"]:focus,
textarea:focus {
  outline: none;
  border-bottom-color: #ff6b6b;
  background: rgba(255, 255, 255, 0.08);
}

input[type="text"]::placeholder,
textarea::placeholder {
  color: rgba(255, 255, 255, 0.5);
}

textarea {
  resize: none;
  min-height: 80px;
  font-family: inherit;
}

input[type="file"] {
  padding: 8px 12px;
  cursor: pointer;
}

small {
  display: block;
  margin-top: 4px;
  color: rgba(255, 255, 255, 0.6);
  font-size: 12px;
}

.tag-input input {
  margin-bottom: 12px;
}

.tags-container {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}

.tag {
  background: rgba(255, 107, 107, 0.2);
  border: 1px solid rgba(255, 107, 107, 0.3);
  padding: 6px 12px;
  border-radius: 16px;
  font-size: 12px;
  display: flex;
  align-items: center;
  gap: 6px;
  color: white;
}

.remove-tag {
  cursor: pointer;
  font-weight: bold;
  font-size: 14px;
  opacity: 0.7;
  transition: opacity 0.2s ease;
}

.remove-tag:hover {
  opacity: 1;
}

.form-actions {
  display: flex;
  gap: 12px;
}

.cancel-btn,
.upload-btn {
  flex: 1;
  padding: 12px 24px;
  border-radius: 8px;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  border: none;
  font-size: 14px;
}

.cancel-btn {
  background: rgba(255, 255, 255, 0.1);
  color: rgba(255, 255, 255, 0.8);
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.cancel-btn:hover:not(:disabled) {
  background: rgba(255, 255, 255, 0.15);
}

.upload-btn {
  background: linear-gradient(135deg, #ff6b6b 0%, #ee5a24 100%);
  color: white;
  box-shadow: 0 4px 16px rgba(255, 107, 107, 0.3);
}

.upload-btn:hover:not(:disabled) {
  background: linear-gradient(135deg, #ff5252 0%, #d63031 100%);
  transform: translateY(-1px);
  box-shadow: 0 6px 20px rgba(255, 107, 107, 0.4);
}

.upload-btn:disabled,
.cancel-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  transform: none;
}

.message {
  margin: 16px 24px 24px 24px;
  padding: 12px 16px;
  border-radius: 8px;
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 14px;
  font-weight: 500;
}

.message.success {
  background: rgba(76, 175, 80, 0.2);
  border: 1px solid rgba(76, 175, 80, 0.3);
  color: #81c784;
}

.message.error {
  background: rgba(244, 67, 54, 0.2);
  border: 1px solid rgba(244, 67, 54, 0.3);
  color: #e57373;
}

.message span {
  font-size: 16px;
}

@media (max-width: 768px) {
  .upload-container {
    margin: 16px;
    max-width: none;
  }
  
  .form-actions {
    flex-direction: column;
  }
}
`]
})
export class UploadVideoPage {
  @Output() onClose = new EventEmitter<void>();
  
  title = '';
  description = '';
  thumbnail!: File;
  video!: File;
  tagInput = '';
  tags: string[] = [];

  uploading = false;
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService) {}

  onThumbnailSelected(event: any) {
    const file = event.target.files[0];
    if (file && file.size > 1024 * 1024 * 5) {
      this.errorMessage = 'Thumbnail is too large (max 5MB).';
      return;
    }
    this.thumbnail = file;
    this.errorMessage = '';
  }

  onVideoSelected(event: any) {
    const file = event.target.files[0];
    if (file && file.size > 1024 * 1024 * 1024) {
      this.errorMessage = 'Video file is too large (max 1GB).';
      return;
    }
    this.video = file;
    this.errorMessage = '';
  }

  addTag() {
    const trimmed = this.tagInput.trim();
    if (trimmed && !this.tags.includes(trimmed)) {
      this.tags.push(trimmed);
    }
    this.tagInput = '';
  }

  removeTag(index: number) {
    this.tags.splice(index, 1);
  }

  upload() {
    if (!this.title.trim()) {
      this.errorMessage = 'Please enter a title.';
      return;
    }
    
    if (!this.video) {
      this.errorMessage = 'Please select a video file.';
      return;
    }
    
    if (!this.thumbnail) {
      this.errorMessage = 'Please select a thumbnail image.';
      return;
    }

    const formData = new FormData();
    formData.append('title', this.title.trim());
    formData.append('description', this.description.trim());
    formData.append('file', this.video);
    formData.append('thumbnail', this.thumbnail);

    this.tags.forEach(tag => formData.append('tags', tag));

    this.uploading = true;
    this.successMessage = '';
    this.errorMessage = '';

    this.api.createVideo(formData).subscribe({
      next: (res) => {
        this.uploading = false;
        this.successMessage = 'Video uploaded successfully!';
        
        // Reset form
        this.resetForm();
        
        // Close upload form after 2 seconds
        setTimeout(() => {
          this.onClose.emit();
        }, 2000);
      },
      error: (err) => {
        this.uploading = false;
        this.errorMessage = err?.error?.message || 'Upload failed. Please try again.';
      }
    });
  }

  resetForm() {
    this.title = '';
    this.description = '';
    this.tags = [];
    this.tagInput = '';
    // Reset file inputs
    const fileInputs = document.querySelectorAll('input[type="file"]') as NodeListOf<HTMLInputElement>;
    fileInputs.forEach(input => input.value = '');
  }

  closeUpload() {
    this.onClose.emit();
  }
}