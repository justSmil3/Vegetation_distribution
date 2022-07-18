using UnityEngine;

public class Bayer_Matrix : MonoBehaviour {
    public static Bayer_Matrix Instance;
    private void Awake() 
{ 
    // If there is an instance.0f, and it's not me.0f, delete myself.
    
    if (Instance != null && Instance != this) 
    { 
        Destroy(this); 
    }
    else 
    { 
        Instance = this; 
    } 
}
public Vector3[] BayerMatrix = new Vector3[]
{
new Vector3(89.0f, 2.0f, 0.8274509803921568f),
new Vector3(116.0f, 204.0f, 0.03529411764705882f),
new Vector3(186.0f, 198.0f, 0.15294117647058825f),
new Vector3(146.0f, 80.0f, 0.18823529411764706f),
new Vector3(47.0f, 161.0f, 0.7490196078431373f),
new Vector3(51.0f, 107.0f, 0.6313725490196078f),
new Vector3(163.0f, 97.0f, 0.6901960784313725f),
new Vector3(130.0f, 120.0f, 0.19215686274509805f),
new Vector3(82.0f, 67.0f, 0.3764705882352941f),
new Vector3(237.0f, 221.0f, 0.5411764705882353f),
new Vector3(55.0f, 80.0f, 0.9f),
new Vector3(188.0f, 179.0f, 0.37254901960784315f),
new Vector3(90.0f, 143.0f, 0.4f),
new Vector3(197.0f, 213.0f, 0.5333333333333333f),
new Vector3(143.0f, 110.0f, 0.9f),
new Vector3(188.0f, 42.0f, 0.11764705882352941f),
new Vector3(174.0f, 222.0f, 0.16470588235294117f),
new Vector3(255.0f, 20.0f, 0.9f),
new Vector3(34.0f, 12.0f, 0.20784313725490197f),
new Vector3(180.0f, 4.0f, 0.03137254901960784f),
new Vector3(231.0f, 238.0f, 0.9f),
new Vector3(20.0f, 191.0f, 0.34901960784313724f),
new Vector3(46.0f, 236.0f, 0.22745098039215686f),
new Vector3(102.0f, 25.0f, 0.49019607843137253f),
new Vector3(3.0f, 33.0f, 0.6901960784313725f),
new Vector3(212.0f, 101.0f, 0.2823529411764706f),
new Vector3(206.0f, 182.0f, 0.16862745098039217f),
new Vector3(119.0f, 6.0f, 0.9f),
new Vector3(105.0f, 183.0f, 0.592156862745098f),
new Vector3(67.0f, 169.0f, 0.6941176470588235f),
new Vector3(69.0f, 250.0f, 0.8666666666666667f),
new Vector3(230.0f, 70.0f, 0.1568627450980392f),
new Vector3(142.0f, 234.0f, 0.1803921568627451f),
new Vector3(47.0f, 49.0f, 0.7490196078431373f),
new Vector3(90.0f, 82.0f, 0.13725490196078433f),
new Vector3(237.0f, 158.0f, 0.8549019607843137f),
new Vector3(144.0f, 43.0f, 0.3176470588235294f),
new Vector3(17.0f, 63.0f, 0.5843137254901961f),
new Vector3(125.0f, 28.0f, 0.792156862745098f),
new Vector3(254.0f, 182.0f, 0.16862745098039217f),
new Vector3(112.0f, 65.0f, 0.25098039215686274f),
new Vector3(121.0f, 231.0f, 0.592156862745098f),
new Vector3(71.0f, 194.0f, 0.9f),
new Vector3(18.0f, 208.0f, 0.18823529411764706f),
new Vector3(133.0f, 211.0f, 0.611764705882353f),
new Vector3(33.0f, 70.0f, 0.8313725490196079f),
new Vector3(134.0f, 252.0f, 0.2235294117647059f),
new Vector3(220.0f, 21.0f, 0.29411764705882354f),
new Vector3(196.0f, 152.0f, 0.050980392156862744f),
new Vector3(229.0f, 111.0f, 0.6f),
new Vector3(42.0f, 207.0f, 0.4f),
new Vector3(163.0f, 46.0f, 0.8980392156862745f),
new Vector3(123.0f, 177.0f, 0.7019607843137254f),
new Vector3(56.0f, 199.0f, 0.3411764705882353f),
new Vector3(251.0f, 59.0f, 0.6352941176470588f),
new Vector3(26.0f, 251.0f, 0.3843137254901961f),
new Vector3(1.0f, 238.0f, 0.8352941176470589f),
new Vector3(66.0f, 124.0f, 0.20784313725490197f),
new Vector3(179.0f, 153.0f, 0.6941176470588235f),
new Vector3(80.0f, 216.0f, 0.00392156862745098f),
new Vector3(155.0f, 135.0f, 0.6549019607843137f),
new Vector3(219.0f, 158.0f, 0.9f),
new Vector3(10.0f, 11.0f, 0.3843137254901961f),
new Vector3(211.0f, 79.0f, 0.6470588235294118f),
new Vector3(77.0f, 32.0f, 0.8117647058823529f),
new Vector3(244.0f, 247.0f, 0.34509803921568627f),
new Vector3(180.0f, 107.0f, 0.36470588235294116f),
new Vector3(131.0f, 150.0f, 0.8941176470588236f),
new Vector3(165.0f, 177.0f, 0.5490196078431373f),
new Vector3(229.0f, 188.0f, 0.788235294117647f),
new Vector3(61.0f, 142.0f, 0.8549019607843137f),
new Vector3(140.0f, 6.0f, 0.10588235294117647f),
new Vector3(212.0f, 232.0f, 0.050980392156862744f),
new Vector3(130.0f, 84.0f, 0.20392156862745098f),
new Vector3(197.0f, 246.0f, 0.8470588235294118f),
new Vector3(225.0f, 54.0f, 0.8313725490196079f),
new Vector3(104.0f, 215.0f, 0.3411764705882353f),
new Vector3(157.0f, 213.0f, 0.5450980392156862f),
new Vector3(97.0f, 116.0f, 0.7686274509803922f),
new Vector3(141.0f, 65.0f, 0.5607843137254902f),
new Vector3(75.0f, 88.0f, 0.9f),
new Vector3(51.0f, 2.0f, 0.8784313725490196f),
new Vector3(216.0f, 212.0f, 0.027450980392156862f),
new Vector3(88.0f, 170.0f, 0.07058823529411765f),
new Vector3(127.0f, 57.0f, 0.7450980392156863f),
new Vector3(207.0f, 9.0f, 0.7450980392156863f),
new Vector3(255.0f, 231.0f, 0.6705882352941176f),
new Vector3(121.0f, 133.0f, 0.5294117647058824f),
new Vector3(88.0f, 246.0f, 0.09019607843137255f),
new Vector3(172.0f, 64.0f, 0.058823529411764705f),
new Vector3(47.0f, 131.0f, 0.6862745098039216f),
new Vector3(78.0f, 155.0f, 0.43137254901960786f),
new Vector3(206.0f, 129.0f, 0.4980392156862745f),
new Vector3(205.0f, 57.0f, 0.5568627450980392f),
new Vector3(28.0f, 136.0f, 0.054901960784313725f),
new Vector3(255.0f, 102.0f, 0.9f),
new Vector3(60.0f, 228.0f, 0.043137254901960784f),
new Vector3(3.0f, 166.0f, 0.8941176470588236f),
new Vector3(73.0f, 4.0f, 0.7803921568627451f),
new Vector3(29.0f, 87.0f, 0.6078431372549019f),
new Vector3(20.0f, 26.0f, 0.11372549019607843f),
new Vector3(111.0f, 90.0f, 0.9f),
new Vector3(94.0f, 55.0f, 0.4196078431372549f),
new Vector3(238.0f, 86.0f, 0.16862745098039217f),
new Vector3(32.0f, 120.0f, 0.00392156862745098f),
new Vector3(0.0f, 126.0f, 0.08235294117647059f),
new Vector3(49.0f, 183.0f, 0.5803921568627451f),
new Vector3(210.0f, 40.0f, 0.19215686274509805f),
new Vector3(168.0f, 195.0f, 0.3254901960784314f),
new Vector3(31.0f, 167.0f, 0.6705882352941176f),
new Vector3(61.0f, 28.0f, 0.792156862745098f),
new Vector3(9.0f, 83.0f, 0.5764705882352941f),
new Vector3(154.0f, 165.0f, 0.4666666666666667f),
new Vector3(194.0f, 82.0f, 0.12549019607843137f),
new Vector3(67.0f, 62.0f, 0.8980392156862745f),
new Vector3(189.0f, 126.0f, 0.8549019607843137f),
new Vector3(104.0f, 153.0f, 0.25882352941176473f),
new Vector3(253.0f, 79.0f, 0.6039215686274509f),
new Vector3(161.0f, 120.0f, 0.7568627450980392f),
new Vector3(171.0f, 242.0f, 0.8901960784313725f),
new Vector3(15.0f, 150.0f, 0.9f),
new Vector3(41.0f, 34.0f, 0.8274509803921568f),
new Vector3(144.0f, 196.0f, 0.01568627450980392f),
new Vector3(195.0f, 27.0f, 0.6313725490196078f),
new Vector3(252.0f, 0.0f, 0.058823529411764705f),
new Vector3(252.0f, 39.0f, 0.3568627450980392f),
new Vector3(7.0f, 253.0f, 0.7254901960784313f),
new Vector3(222.0f, 134.0f, 0.16862745098039217f),
new Vector3(5.0f, 217.0f, 0.5529411764705883f),
new Vector3(245.0f, 201.0f, 0.5529411764705883f),
new Vector3(118.0f, 248.0f, 0.23921568627450981f),
new Vector3(191.0f, 64.0f, 0.9f),
new Vector3(157.0f, 6.0f, 0.8588235294117647f),
new Vector3(236.0f, 26.0f, 0.11764705882352941f),
new Vector3(50.0f, 65.0f, 0.4392156862745098f),
new Vector3(95.0f, 98.0f, 0.9f),
new Vector3(212.0f, 253.0f, 0.28627450980392155f),
new Vector3(88.0f, 196.0f, 0.027450980392156862f),
new Vector3(254.0f, 133.0f, 0.4823529411764706f),
new Vector3(9.0f, 102.0f, 0.8431372549019608f),
new Vector3(47.0f, 254.0f, 0.9f),
new Vector3(232.0f, 8.0f, 0.00784313725490196f),
new Vector3(179.0f, 78.0f, 0.8980392156862745f),
new Vector3(108.0f, 47.0f, 0.35294117647058826f),
new Vector3(157.0f, 29.0f, 0.5411764705882353f),
new Vector3(140.0f, 176.0f, 0.058823529411764705f),
new Vector3(103.0f, 235.0f, 0.6784313725490196f),
new Vector3(0.0f, 193.0f, 0.25098039215686274f),
new Vector3(29.0f, 229.0f, 0.5450980392156862f),
new Vector3(78.0f, 104.0f, 0.24313725490196078f),
new Vector3(238.0f, 125.0f, 0.47843137254901963f),
new Vector3(160.0f, 230.0f, 0.0784313725490196f),
new Vector3(193.0f, 231.0f, 0.5803921568627451f),
new Vector3(210.0f, 197.0f, 0.4549019607843137f),
new Vector3(103.0f, 131.0f, 0.6745098039215687f),
new Vector3(26.0f, 47.0f, 0.4f),
new Vector3(34.0f, 151.0f, 0.39215686274509803f),
new Vector3(7.0f, 49.0f, 0.7372549019607844f),
new Vector3(137.0f, 134.0f, 0.8431372549019608f),
new Vector3(210.0f, 145.0f, 0.4392156862745098f),
new Vector3(255.0f, 162.0f, 0.9f),
new Vector3(27.0f, 105.0f, 0.6980392156862745f),
new Vector3(116.0f, 110.0f, 0.09803921568627451f),
new Vector3(202.0f, 167.0f, 0.403921568627451f),
new Vector3(198.0f, 112.0f, 0.23529411764705882f),
new Vector3(240.0f, 175.0f, 0.3333333333333333f),
new Vector3(255.0f, 214.0f, 0.9f),
new Vector3(162.0f, 151.0f, 0.39215686274509803f),
new Vector3(78.0f, 49.0f, 0.4980392156862745f),
new Vector3(14.0f, 117.0f, 0.4823529411764706f),
new Vector3(180.0f, 255.0f, 0.34901960784313724f),
new Vector3(174.0f, 33.0f, 0.4980392156862745f),
new Vector3(174.0f, 135.0f, 0.4196078431372549f),
new Vector3(151.0f, 247.0f, 0.6588235294117647f),
new Vector3(76.0f, 234.0f, 0.11764705882352941f),
new Vector3(81.0f, 130.0f, 0.8156862745098039f),
new Vector3(87.0f, 19.0f, 0.6745098039215687f),
new Vector3(162.0f, 77.0f, 0.4588235294117647f),
new Vector3(221.0f, 174.0f, 0.8549019607843137f),
new Vector3(129.0f, 192.0f, 0.7529411764705882f),
new Vector3(16.0f, 175.0f, 0.3333333333333333f),
new Vector3(0.0f, 63.0f, 0.3333333333333333f),
new Vector3(63.0f, 45.0f, 0.7294117647058823f),
new Vector3(44.0f, 92.0f, 0.0392156862745098f),
new Vector3(92.0f, 38.0f, 0.10588235294117647f),
new Vector3(228.0f, 254.0f, 0.09803921568627451f),
new Vector3(142.0f, 25.0f, 0.49411764705882355f),
new Vector3(103.0f, 254.0f, 0.9f),
new Vector3(1.0f, 143.0f, 0.5843137254901961f),
new Vector3(118.0f, 161.0f, 0.48627450980392156f),
new Vector3(237.0f, 43.0f, 0.6196078431372549f),
new Vector3(171.0f, 17.0f, 0.7019607843137254f),
new Vector3(103.0f, 9.0f, 0.7411764705882353f),
new Vector3(22.0f, 0.0f, 0.23529411764705882f),
new Vector3(237.0f, 141.0f, 0.5411764705882353f),
new Vector3(48.0f, 19.0f, 0.3137254901960784f),
new Vector3(130.0f, 101.0f, 0.4549019607843137f),
};}