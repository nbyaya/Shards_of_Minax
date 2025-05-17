using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;
using Server.Targeting;


namespace Server.Mobiles
{
    [CorpseName("a thornlash corpse")]
    public class Thornlash : BaseCreature
    {
        // Unique Hue
        private const int UniqueHue = 0x8A4; // Example unique hue (a thorny green/brown)

        [Constructable]
        public Thornlash()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Thornlash"; // Unique Name
            Body = 314;         // Ravager Body
            BaseSoundID = 357;  // Ravager Sound ID

            // Significantly Increased Stats
            SetStr(400, 500);
            SetDex(150, 175);
            SetInt(100, 125);

            // Increased Hits and Damage
            SetHits(500, 600);
            SetDamage(20, 30);

            // Damage Types (Physical and some Poison)
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            // Resistances (Adjusted for Wilderness/Plant-like theme)
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 30, 40); // Vulnerable to Fire?
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 70, 80); // High Poison Resistance
            SetResistance(ResistanceType.Energy, 40, 50);

            // Increased Skill Levels
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 95.0, 115.0);
            SetSkill(SkillName.Poisoning, 80.0, 100.0); // Added Poisoning for potential poison effects

            // Fame and Karma
            Fame = 8000;
            Karma = -8000;

            // Virtual Armor
            VirtualArmor = 60;

            // Weapon Abilities (Examples)
            SetWeaponAbility(WeaponAbility.BleedAttack); // Added Bleed
            SetWeaponAbility(WeaponAbility.Dismount);

            // Apply unique hue
            Hue = UniqueHue;
        }

        public Thornlash(Serial serial)
            : base(serial)
        {
        }

        // --- Unique Abilities ---

        // AOE Attack: Thorn Barrage (Windup)
        private DateTime _NextThornBarrage;
        private TimeSpan ThornBarrageCooldown = TimeSpan.FromSeconds(15.0); // Cooldown for the AOE
        private TimeSpan ThornBarrageWindup = TimeSpan.FromSeconds(3.0);    // Windup time

        // Passive: Thorny Hide
        private const double ThornyHideChance = 0.25; // 25% chance to activate on melee hit
        private const int ThornyHideDamage = 5;      // Damage dealt back

        // Melee Effect: Gouging Thorns
        private const double GougingThornsChance = 0.30; // 30% chance to apply effect on melee hit

        public override void OnActionCombat()
        {
            // Ensure Combatant is a Mobile before accessing Mobile-specific properties/methods
            if (Combatant is Mobile target)
            {
                // Check for Thorn Barrage AOE attack
                if (DateTime.UtcNow >= _NextThornBarrage && InRange(target, 10) && CanBeHarmful(target) && InLOS(target))
                {
                    PerformThornBarrage(target);
                    _NextThornBarrage = DateTime.UtcNow + ThornBarrageCooldown;
                }
            }

            base.OnActionCombat(); // Continue with normal combat actions
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            // Thorny Hide Passive
            if (ThornyHideChance >= Utility.RandomDouble() && attacker != this && CanBeHarmful(attacker))
            {
                DoHarmful(attacker);
                AOS.Damage(attacker, this, ThornyHideDamage, 100, 0, 0, 0, 0);
                attacker.SendAsciiMessage("You are pricked by Thornlash's thorny hide!");
                PlaySound(0x2A1); // Example sound for thorns hit (adjust as needed)
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // Ensure defender is a Mobile before accessing Mobile-specific properties/methods
             if (defender is Mobile target)
             {
                // Gouging Thorns Melee Effect
                if (GougingThornsChance >= Utility.RandomDouble() && CanBeHarmful(target))
                {
                    DoHarmful(target);

                    // Apply Bleed or Slow effect
                    if (Utility.RandomBool()) // 50% chance for Bleed
                    {
                        target.ApplyPoison(this, Poison.Lethal); // Apply Lethal Poison
                        target.SendAsciiMessage("Thornlash's attack poisons you!");
                    }
                    else // 50% chance for Slow
                    {
                        target.Paralyze(TimeSpan.FromSeconds(2.0)); // Brief paralysis/slow
                        target.SendAsciiMessage("Thornlash's attack slows your movements!");
                    }

                    PlaySound(0x5E4); // Example sound for special attack (adjust as needed)
                }
             }
        }


        public void PerformThornBarrage(Mobile target)
        {
            // Windup Animation and Message
            this.Animate(11, 5, 1, true, false, 0); // Example animation (adjust ID/settings)
            this.PublicOverheadMessage(Network.MessageType.Regular, UniqueHue, true, "*Thornlash prepares a thorn barrage!*");
            PlaySound(0x5A1); // Example windup sound (adjust as needed)

            // Delay for windup
            Timer.DelayCall(ThornBarrageWindup, () =>
            {
                // Actual AOE Attack
                PlaySound(0x2D3); // Example attack sound (adjust as needed)
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5052, 0); // Example large particle effect

                int aoeRange = 4; // AOE radius in tiles
                int baseAoEDamage = Utility.RandomMinMax(25, 40);

                foreach (Mobile mob in GetMobilesInRange(aoeRange))
                {
                    // Ensure target is a Mobile before accessing Mobile-specific properties/methods
                    if (mob is Mobile aoeTarget)
                    {
                        if (aoeTarget != this && CanBeHarmful(aoeTarget))
                        {
                            DoHarmful(aoeTarget);

                            // Calculate damage reduction based on physical and poison resistance
                            int physicalResistance = aoeTarget.PhysicalResistance;
                            int poisonResistance = aoeTarget.PoisonResistance;

                            // Simple combined resistance calculation (can be more complex)
                            double totalResistance = (physicalResistance * 0.7) + (poisonResistance * 0.3); // 70% physical, 30% poison

                            int reducedDamage = (int)(baseAoEDamage * (1.0 - (totalResistance / 100.0)));

                            // Ensure damage is at least 1
                            reducedDamage = Math.Max(1, reducedDamage);

                            AOS.Damage(aoeTarget, this, reducedDamage, 70, 0, 0, 30, 0); // Damage split physical/poison

                            // Optional: Apply a short-duration debuff or DOT to AOE targets
                            // Example: Apply a weak bleed effect
                            // BleedAttack.BeginBleed(aoeTarget, this);
                            // Example: Apply a weak poison
                            aoeTarget.ApplyPoison(this, Poison.Regular);
                            aoeTarget.SendAsciiMessage("You are struck by venomous thorns!");

                             // Visual/Sound for hit target
                            aoeTarget.FixedParticles(0x374A, 10, 15, 5021, UniqueHue, 0, EffectLayer.Waist); // Example particle on hit
                            aoeTarget.PlaySound(0x22F); // Example hit sound
                        }
                    }
                }

                this.PublicOverheadMessage(Network.MessageType.Regular, UniqueHue, true, "*Thornlash unleashes a barrage of thorns!*");
            });
        }

        // --- Standard Overrides ---

        public override bool CanRummageCorpses { get { return true; } }

        public override int TreasureMapLevel { get { return 5; } } // Higher level map

        public override int Meat { get { return 3; } } // More meat than Ravager

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.UltraRich); // Higher quality loot
            this.AddLoot(LootPack.HighScrolls); // Add scrolls
            this.AddLoot(LootPack.Gems); // Add gems

            // Optional: Add a chance for a unique item
            if (Utility.RandomDouble() < 0.01) // 1 in 100 chance
            {
                this.PackItem(new Item(0x9EC) { Name = "Thornlash's Bark Fragment", Hue = UniqueHue }); // Example unique item
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}