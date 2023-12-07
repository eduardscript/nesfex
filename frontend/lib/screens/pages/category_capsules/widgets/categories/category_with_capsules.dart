import 'package:flutter/material.dart';
import 'package:frontend/entities/category.dart';

import 'widgets.dart';

class CategoryWithCapsulesTile extends StatelessWidget {
  const CategoryWithCapsulesTile({super.key, required this.categories});

  final List<Category> categories;

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
