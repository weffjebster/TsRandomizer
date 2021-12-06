using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Timespinner.Core;
using Timespinner.GameAbstractions.Inventory;
using Timespinner.GameAbstractions.Saving;
using Timespinner.GameObjects.BaseClasses;
using TsRandomizer.Extensions;
using TsRandomizer.IntermediateObjects;

namespace TsRandomizer.Randomisation
{
    class AttackElements
    {
        public EDamageElement MainElement { get; set; }
        public EDamageElement SubElement { get; set; }
    }

    static class ElementManager
    {
        public static Dictionary<Type, EDamageElement> ElementLookup = new Dictionary<Type, EDamageElement>();
        public static Dictionary<SlotAndType, AttackElements> OrbElementLookup = new Dictionary<SlotAndType, AttackElements>();

        public static void PopulateElementLookups(Seed seed)
        {
            ElementLookup.Clear();
            OrbElementLookup.Clear();
            var random = new Random((int)seed.Id);
            foreach(var type in ProjectileManager.DamagingProjectiles)
            {
                var elementId = random.Next(1, 9); //omit None
                if (!ElementLookup.ContainsKey(type.ProjectileType))
                {
                    var element = (EDamageElement)Enum.GetValues(typeof(EDamageElement)).GetValue(elementId);

                    if(!OrbElementLookup.ContainsKey(type.SlotAndType))
                    {
                        
                        OrbElementLookup.Add(type.SlotAndType, new AttackElements() {
                            MainElement = element
                        });
                    }                    
                    ElementLookup.Add(type.ProjectileType, element);
                }
            }

            foreach(var type in ProjectileManager.DamagingSubProjectiles)
            {
                var elementId = random.Next(1, 9); //omit None
                var parentProjectile = type.ParentProjectile;
                var parentElements = OrbElementLookup.FirstOrDefault(o => parentProjectile.SlotAndType.Equals(o.Key));
                if (!ElementLookup.ContainsKey(type.ProjectileType))
                {
                    EDamageElement element;
                    if(type.ShouldShareDamageType && parentElements.Value != null)
                    {
                        element = parentElements.Value.MainElement;
                    }
                    else
                    {
                        element = (EDamageElement)Enum.GetValues(typeof(EDamageElement)).GetValue(elementId);
                    }

                    if (!OrbElementLookup.Any(oe => type.SlotAndType.Equals(oe.Key)))
                    {
                        OrbElementLookup.Add(type.SlotAndType, new AttackElements()
                        {
                            MainElement = element
                        });
                    }
                    if(parentElements.Value != null)
                    {
                        parentElements.Value.SubElement = element;
                    }                    
                    ElementLookup.Add(type.ProjectileType, element);
                }
            }
        }

        public static void SetElement(Projectile projectile)
        {
            Type ProjectileType = projectile.GetType();
            var damageElement = ProjectileType.GetField("_damageElement", BindingFlags.Instance | BindingFlags.NonPublic);
            if (ElementLookup.TryGetValue(ProjectileType, out EDamageElement storedElement))
            {
                damageElement.SetValue(projectile, storedElement);
            }
        }

        public static string GetElementString(EOrbSlot orbSlot, EInventoryOrbType orbType)
        {
            SlotAndType inputType = new SlotAndType { Slot = orbSlot, Type = orbType };
            var elements = OrbElementLookup.FirstOrDefault(o => o.Key.Slot == orbSlot && o.Key.Type == orbType);
            if(elements.Value != null)
            {
                return (elements.Value.SubElement != EDamageElement.None && elements.Value.MainElement != elements.Value.SubElement)
                    ? $"({elements.Value.MainElement}/{elements.Value.SubElement})"
                    : $"({elements.Value.MainElement})";
            }
            return "";
        }

        public static IEnumerable<SlotAndType> GetFireSources()
        {
            var fireSources = OrbElementLookup.Where(o => o.Value.MainElement == EDamageElement.Fire || o.Value.SubElement == EDamageElement.Fire);
            return fireSources.Select(fs => fs.Key);
        }
    }
}
