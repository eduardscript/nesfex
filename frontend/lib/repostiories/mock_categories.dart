import 'package:frontend/entities/capsule.dart';
import 'package:frontend/entities/category.dart';

const mockCategories = [
  Category(name: "Limited Editions", capsules: [
    Capsule(
      name: "Vertuo Advent Calendar",
      intensity: 0,
      price: 50.90,
      imageUrl:
          "https://www.nespresso.com/ecom/medias/sys_master/public/28918741598238/C-1192-Responsive-PLPimage-320320.png",
      label: "Limited Editions",
    ),
    Capsule(
      name: "Festive Black Double Espresso",
      intensity: 7,
      price: 0.61,
      imageUrl:
          "https://www.nespresso.com/ecom/medias/sys_master/public/29161053782046/C-1160-plpImage-2x.png",
      label: "Limited Editions",
    ),
    Capsule(
      name: "Frosted Caramel Nuts",
      intensity: 0,
      price: 0.71,
      imageUrl:
          "https://www.nespresso.com/ecom/medias/sys_master/public/28823610556446/C-1165-plpImage.png",
      label: "Limited Editions",
    ),
    Capsule(
      name: "Seasonal Delight Spices",
      intensity: 0,
      price: 0.71,
      imageUrl:
          "https://www.nespresso.com/ecom/medias/sys_master/public/28823977951262/C-1170-plpImage.png",
      label: "Limited Editions",
    ),
  ]),
  Category(name: "Ristretto", capsules: [
    Capsule(
      name: "Ristretto Intenso",
      intensity: 12,
      price: 0.47,
      label: "Ristretto 25ml",
      imageUrl:
          "https://www.nespresso.com/ecom/medias/sys_master/public/27750715260958.png",
    ),
    Capsule(
      name: "Ristretto Classico",
      intensity: 9,
      price: 0.45,
      label: "Ristretto 25ml",
      imageUrl:
          "https://www.nespresso.com/ecom/medias/sys_master/public/27750724108318/C-1131-ResponsivePLPImage.png",
    ),
  ]),
];
