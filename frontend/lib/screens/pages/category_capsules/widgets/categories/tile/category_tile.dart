import 'package:flutter/material.dart';
import 'package:flutter_slidable/flutter_slidable.dart';
import 'package:frontend/entities/category.dart';
import 'package:frontend/screens/pages/category_capsules/widgets/capsules/dialog/action_dialog.dart';
import 'package:frontend/screens/pages/category_capsules/widgets/categories/tile/category_tite_title.dart';

import '../../capsules/widgets.dart';

class CategoryTile extends StatelessWidget {
  final Category category;

  const CategoryTile({super.key, required this.category});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        CategoryTileTitle(category: category),
        CategoryCapsules(category: category),
      ],
    );
  }
}

class CategoryCapsules extends StatelessWidget {
  const CategoryCapsules({
    super.key,
    required this.category,
  });

  final Category category;

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      physics: const ClampingScrollPhysics(),
      shrinkWrap: true,
      itemCount: category.capsules.length,
      itemBuilder: (context, index) {
        return Slidable(
          startActionPane: ActionPane(
            motion: const ScrollMotion(),
            children: [
              SlidableAction(
                onPressed: (context) => {
                  showDialog(
                    context: context,
                    builder: (context) => const ActionDialog(
                      title: 'How many capsules do you want to add?',
                      buttonText: 'Add',
                    ),
                  )
                },
                backgroundColor: Colors.black87,
                foregroundColor: Colors.white,
                icon: Icons.add,
                label: 'Add',
              ),
              SlidableAction(
                onPressed: (context) => {
                  showDialog(
                    context: context,
                    builder: (context) => const ActionDialog(
                      title: 'How many capsules do you want to remove?',
                      buttonText: 'Remove',
                    ),
                  )
                },
                backgroundColor: Colors.black54,
                foregroundColor: Colors.white,
                icon: Icons.remove,
                label: 'Remove',
              ),
            ],
          ),
          endActionPane: ActionPane(
            motion: const ScrollMotion(),
            children: [
              SlidableAction(
                onPressed: (context) => {},
                backgroundColor: Colors.black,
                foregroundColor: Colors.white,
                icon: Icons.local_drink,
                label: 'Drinked',
              ),
            ],
          ),
          child: CapsuleTile(capsule: category.capsules[index]),
        );
      },
    );
  }
}
