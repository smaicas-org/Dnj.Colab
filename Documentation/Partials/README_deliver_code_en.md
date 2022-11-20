1 - Create a GitHub account

2 - Go to [repository page](https://github.com/smaicas-org/Dnj.Colab) and "Create a new Fork"

![](../Assets/fork_1.png)

![](../Assets/fork_2.png)

It will create a copy of the original repository in your account.
Navigate to the recently created repository and press the "Code" button.
Copy the url to the repository.

3 - Clone the repository manually or using an IDE such Visual Studio:

- Manually:	
REPOSITORY URL must have the format  https://USERNAME@github.com/source/path.git to specify the username.
```
git clone {REPOSITORY URL}
```
- Using Visual Studio:

![](../Assets/vs_clone_1.png)

![](../Assets/vs_clone_2.png)

4 - Implement your changes in the project allocated in dev branch of your forked repository. 

5 - Once your change are ready to merge with the original project.
Create a new branch with base in 'dev'. 
(You can do it manually or you can use software as Fork, SourceTree or your desired IDE)

- Manually:
```
git checkout dev
```
```
git switch -c "feature/mybranchname"
```
- Using Visual Studio:

![](../Assets/vs_newbranch_1.png)

![](../Assets/vs_newbranch_2.png)

6 - Commit changes to you recently created branch and push it to remote repository.
- Manually:
```
git add .
```
```
git commit -m "COMMIT MESSAGE WITH LITTLE DESCRIPTION OF THE CHANGES"
```
```
git push origin "feature/mybranchname"
```
_It will prompt for login_ 
    
7 - Go to github and initiate a pull request

![](../Assets/pullrequest_1.png)

![](../Assets/pullrequest_2.png)

![](../Assets/pullrequest_3.png)

Your Pull Request will be reviewed and you will get information if the reviewer needs
you to change some code or it's approved and the merge will be.