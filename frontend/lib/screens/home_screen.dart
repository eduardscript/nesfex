import 'package:flutter/material.dart';
import 'package:frontend/repostiories/mock_categories.dart';
import 'package:frontend/screens/pages/category_capsules/widgets/categories/category_with_capsules.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        centerTitle: true,
        backgroundColor: Colors.black,
        titleTextStyle: const TextStyle(
          color: Colors.white,
          fontSize: 20,
          fontWeight: FontWeight.bold,
        ),
        title: const Text('NESPREX'),
      ),
      body: const CategoryWithCapsulesTile(categories: mockCategories),
    );
  }
}
