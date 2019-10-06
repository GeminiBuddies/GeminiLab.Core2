# Branch Strategy

## 1. List of Branches

### 1.1 Develop branch (`dev`)

The develop branch records the last changes in development progress.

On this branch developers can:

- Fix bugs.
- Modify APIs.
- Do code cleanup and refactoring.
- Write tests.
- Delete existing features.

and cannot:

- Add new features. Do it on `f/<name>` instead.
- Create tags (or publish releases).

This branch persists locally and remotely. Following branches can be merged into develop branch: feature branches(new features), release branch(bugfixes) and hotfix branches(bugfixes).

### 1.2 Feature branches (`f/<name>`)

Feature branches are branches where new features are developed.

On these branches developers can:

- Add new features and their tests.
- Modify APIs of new features.

and cannot:

- Create tags (or publish releases).

Feature branches always diverge from develop branch and will be deleted after merged back into develop branch when development of new features is finished.

### 1.3 Release branch (`release`)

Release branch is the branch where preview releases are published and pre-release tests are implemented.

On this branch developers can:

- Fix bugs.
- Publish preview releases.
- Do code cleanup and refactoring. (Though the develop branch is a better choice.)

and cannot:

- Add new features.
- Delete existing features. UNLESS it's due to a security issue.
- Modify APIs.

Release branch always diverges from develop branch and will be deleted after being merged into master branch.

### 1.4 Master branch (`master`)

Master branch is the most stable branch where stable releases are published.

On this branch developers can:

- Publish stable releases.
- Merge from release branch and hotfix branches.

and cannot:

- Commit anything. Use branch `hotfix` to fix bugs.

### 1.5 Hotfix branches (`hotfix`, `hotfix/<name>`)

Hotfix branches are the place where bugs found in master branch get fixed.

Hotfix branches diverge only from master branch. This branch should be deleted after being merged into master branch and develop branch (and release branch if necessary) after the bug is fixed.
