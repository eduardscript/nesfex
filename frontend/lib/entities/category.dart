import 'package:frontend/entities/capsule.dart';

class Category {
  final String name;
  final List<Capsule> capsules;

  const Category({
    required this.name,
    required this.capsules,
  });
}
