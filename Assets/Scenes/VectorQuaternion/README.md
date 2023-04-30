# Vector and Quaternion Test
Black arrow : camera look  
White arrow : controller input  
Red arrow : character look  
![output](https://user-images.githubusercontent.com/60992540/235331500-b162d81b-b55f-4ffe-8ef7-acebafcbd912.gif)

### 世界座標とカメラ基準座標の変換のテスト
コントローラーの入力を "世界座標" ではなく "黒矢印のカメラが向いている方向" を基準にして適用する。  
コントローラーの入力は白矢印、実際にカメラ基準に変換されたものは赤矢印。

カメラが世界座標 (1, 0, 0) を向いているときはコントローラーの右入力が世界座標 (0, 0, -1) になる。