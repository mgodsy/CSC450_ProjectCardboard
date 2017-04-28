// Copyright 2015 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class vrHUD : MonoBehaviour {
  private const string DISPLAY_TEXT_FORMAT = "MP: {0}\n{1}";
  
  private Text textField;
  string curCoor;
  GameObject targetData;
  GameObject megaPoint;
  DataPoint dataPoint;
  private int mpId; 
  TargetDataPoint targetDataPoint;
  string currentDataPoint;

  public Camera cam;

  void Awake() {
    textField = GetComponent<Text>();
  }

  void pullCoordinates(){
      megaPoint = GameObject.Find(targetDataPoint.GetDataPoint());
      dataPoint = megaPoint.GetComponent<DataPoint>();
      curCoor = dataPoint.Coordinates;
      mpId = dataPoint.id;
    }

  void Start() {
    targetData = GameObject.Find("TargetData");
    targetDataPoint = targetData.GetComponent<TargetDataPoint>();
	currentDataPoint = null;

    pullCoordinates();

    if (cam == null) {
       cam = Camera.main;
    }

    if (cam != null) {
      // Tie this to the camera, and do not keep the local orientation.
      transform.SetParent(cam.GetComponent<Transform>(), true);
    }
  }

  void LateUpdate() {
    
    textField.text = string.Format(DISPLAY_TEXT_FORMAT,
        mpId, curCoor);
  }
}
