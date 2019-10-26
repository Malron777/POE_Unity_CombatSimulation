﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    Time.timeScale = 0;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void Play()
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene("Main");
    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void Quit()
  {
    Application.Quit();
  }

  public void ShowDetails()
  {

  }
}
