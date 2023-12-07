import 'package:flutter/material.dart';
import 'package:frontend/entities/category.dart';

class CategoryTileTitle extends StatelessWidget {
  const CategoryTileTitle({
    super.key,
    required this.category,
  });

  final Category category;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 20),
      child: Text(
        category.name,
        style: const TextStyle(
          fontSize: 16,
          fontWeight: FontWeight.w300,
        ),
      ),
    );
  }
}
