import 'package:flutter/material.dart';
import 'package:frontend/entities/capsule.dart';
import 'package:frontend/entities/category.dart';

const categories = [
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

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      initialIndex: 1,
      length: 2,
      child: Scaffold(
        appBar: AppBar(
          title: const Text('Boutique'),
          bottom: const TabBar(
            tabs: [
              Tab(icon: Icon(Icons.recycling)),
              Tab(icon: Icon(Icons.card_travel)),
            ],
          ),
        ),
        body: const TabBarView(
          children: [
            CategoryCapsulesScreen(),
            Center(child: Text('Buy')),
          ],
        ),
      ),
    );
  }
}

class CategoryCapsulesScreen extends StatelessWidget {
  const CategoryCapsulesScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      shrinkWrap: true,
      physics: const ClampingScrollPhysics(),
      itemCount: categories.length,
      itemBuilder: (context, index) {
        return CategoryTile(category: categories[index]);
      },
    );
  }
}

class CategoryTile extends StatelessWidget {
  final Category category;

  const CategoryTile({super.key, required this.category});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 20),
          child: Text(
            category.name,
            style: const TextStyle(
              fontSize: 16,
              fontWeight: FontWeight.w300,
            ),
          ),
        ),
        ListView.builder(
          physics: const ClampingScrollPhysics(),
          shrinkWrap: true,
          itemCount: category.capsules.length,
          itemBuilder: (context, index) {
            return Column(children: [
              CapsuleCard(
                capsule: category.capsules[index],
              ),
              const Divider()
            ]);
          },
        ),
      ],
    );
  }
}

class CapsuleCard extends StatelessWidget {
  final Capsule capsule;

  const CapsuleCard({super.key, required this.capsule});

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.symmetric(vertical: 5, horizontal: 10),
          child: Image.network(
            capsule.imageUrl,
            width: 50,
          ),
        ),
        Column(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Container(
              padding: const EdgeInsets.symmetric(
                vertical: 2,
                horizontal: 8,
              ),
              color: _determineLabelBackgroundColor(),
              child: Text(
                capsule.label,
                style: const TextStyle(
                  fontSize: 8,
                  fontWeight: FontWeight.w500,
                  color: Color(0xFFffffff),
                ),
              ),
            ),
            Text(
              capsule.name,
              style: const TextStyle(
                fontSize: 12,
                fontWeight: FontWeight.bold,
              ),
            ),
            if (capsule.intensity != 0)
              Text.rich(
                TextSpan(
                  text: 'Intensity ',
                  style: const TextStyle(
                    color: Color(0xFF876c43),
                    fontSize: 11,
                  ),
                  children: [
                    TextSpan(
                      text: '${capsule.intensity}',
                      style: const TextStyle(
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ],
                ),
              ),
          ],
        ),
        const Spacer(),
        Row(
          children: [
            Text(
              '€ ${capsule.price.toStringAsFixed(2)}',
              style: const TextStyle(
                fontSize: 12,
                color: Color(0xFF3d8705),
                fontWeight: FontWeight.bold,
              ),
            ),
            const SizedBox(width: 10),
            InkWell(
              onTap: () {},
              child: Ink(
                height: 40,
                width: 40,
                decoration: BoxDecoration(
                  color: const Color(0xFF3d8705),
                  borderRadius: BorderRadius.circular(3),
                ),
                child: const Icon(
                  Icons.add,
                  color: Colors.white,
                  size: 20,
                ),
              ),
            )
          ],
        ),
        const SizedBox(width: 10),
      ],
    );
  }

  Color _determineLabelBackgroundColor() {
    if (capsule.label == 'Limited Editions') {
      return const Color(0xFF24454f);
    }

    return const Color(0xFF9b9b9b);
  }
}
