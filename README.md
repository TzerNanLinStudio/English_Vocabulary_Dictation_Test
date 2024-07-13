# English Vocabulary Dictation Test

# 1. Introduction

This project was developed as a side project while I was preparing for the IELTS exam, to help myself review for the test. When I was private tutor, it was also used as teaching material for a university student. The student developed an English interface under my guidance, while mine is in Chinese.

I will briefly introduce the functionality and usage of this software. Everyone is welcome to download and test it from GitHub. If there are any questions, feel free to email me.

# 2. Functionality

## 2.1. Save Words

![Image Error](./Other/Image/image_01.png)

![Image Error](./Other/Image/image_02.png)

![Image Error](./Other/Image/image_03.png)

The three images above demonstrate the basic functionalities of the "編輯區" tab-page. Users can select notes from different dates, add new English words, parts of speech, and Chinese meanings to their notes. When users click on the English word field, the system will pronounce the word and display its basic English definition. Therefore, users can save unfamiliar words on this tab-page and review them.

## 2.2. Test Words

![Image Error](./Other/Image/image_04.png)

![Image Error](./Other/Image/image_05.png)

![Image Error](./Other/Image/image_06.png)

The three images above demonstrate the basic functionalities of the "測試區" tab-page. Users can choose different notes for vocabulary dictation tests. The system will randomly play vocabulary words from the notes, and users need to spell the words in the provided field. After completing the test, the system will display the correct answers, accuracy rate, and past performance. Therefore, users can use this tab-page to test whether they have learned unfamiliar vocabulary words.

# 3. References

 - Integrated Development Environment: Visual Studio 2019
 - Frontend Development: WPF (Windows Presentation Foundation)
 - Backend Development: C#

# 4. Future Work

 - Use the Config class to record the parameters required for software operation to ensure that the software's state when opened is consistent with its state when it was last closed.
 - Use the LogRecorder to record the software's running status.
 - Use databases to store unfamiliar words, such as MySQL and SQLite.
 - Establish test and GitHub Actions workflows related to CI/CD.
