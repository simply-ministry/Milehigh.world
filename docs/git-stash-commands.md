# Git Stash Commands Cheat Sheet

Save your local modifications to a new stash:
```sh
git stash
```

List all stashes:
```sh
git stash list
```

Apply the most recent stash and keep it in the stash list:
```sh
git stash apply
```

Apply and remove the most recent stash:
```sh
git stash pop
```

Show changes in a specific stash:
```sh
git stash show -p stash@{0}
```