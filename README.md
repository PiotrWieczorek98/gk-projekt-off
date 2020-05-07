# gk-projekt-off

## How to create branch
1. `cd 'Project Directory'`
2. `git checkout -b 'name of new branch'`
3. `git status` can help you check if you are on a correct branch.
4. If you want to go into different branch -> `git checkout 'branch'`
5. Normally do your commits, but push with -> `git push origin 'name of the branch to push to'`
6. If you want to merge branch into master -> `git checkout master -> git merge 'name of branch to merge'`
7. If you want to download certain branch -> `git pull https://github.com/PiotrWieczorek98/gk-projekt-off.git 'branch'`

## How to delete your commit
1. `cd 'Project Directory'`
2. `git rebase -i 'name of commit just before the last commit you want to remove'` (7 characters -> click _ commits on github and check the name)
3. Here you can change `'pick'` into correct name from list of commends that are printed in cmd, you can do it in more than one commit.
4. Click esc, than write `':w'` to save, after that write `':q'` to quit.
5. `git push -f origin 'branch'`

6. __If you have some problems with versions of files after that, that's your problem :).__

## Things to do:
- [] Create menu.
- [] Create scenes.
     - [] Start scene.
     - [] Scenes in between.
     - [] Final scene.
- [] Create key, to look for.
