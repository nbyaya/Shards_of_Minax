using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a putrid ridgeback corpse")]
    public class PutridRidgeback : BaseCreature
    {
        private DateTime _nextAcidSpit;
        private DateTime _nextGasCloud;
        private int _frenzyHits;
        private DateTime _frenzyExpire;

        [Constructable]
        public PutridRidgeback()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a Putrid Ridgeback";
            this.Body = 0x3EBA;       // Using the same body as the standard Ridgeback.
            this.BaseSoundID = 0x3F3;
            this.Hue = 0x497;         // Unique putrid green hue.

            // Enhanced Attributes
            this.SetStr(150, 200);
            this.SetDex(120, 140);
            this.SetInt(80, 100);

            this.SetHits(300, 350);
            this.SetDamage(10, 20);

            // Damage types: mostly physical with a boost of poison damage.
            this.SetDamageType(ResistanceType.Physical, 70);
            this.SetDamageType(ResistanceType.Poison, 30);

            // Resistances: notably high poison resistance.
            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 20, 30);
            this.SetResistance(ResistanceType.Poison, 60, 80);
            this.SetResistance(ResistanceType.Energy, 20, 30);

            this.SetSkill(SkillName.MagicResist, 75.0, 90.0);
            this.SetSkill(SkillName.Tactics, 80.0, 95.0);
            this.SetSkill(SkillName.Wrestling, 85.0, 100.0);

            this.Fame = 5000;
            this.Karma = -5000;
            this.VirtualArmor = 50;


        }

        public PutridRidgeback(Serial serial)
            : base(serial)
        {
        }

        // -- Unique Ability: Corrosive Acid Spit --
        // Occasionally the Putrid Ridgeback spits acid that damages and temporarily debuffs a foe.
        public void AcidSpit(Mobile m)
        {
            if (m == null)
                return;
            if (!(m is Mobile target)) // Always check before using Mobile-specific members.
                return;

            DoHarmful(m);
            target.PlaySound(0x642); // Custom acid spit sound.
            // Create visual acid spit effect; adjust particle IDs as needed.

            // Simulate travel time of the acid projectile.
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), delegate()
            {
                if (m == null || m.Deleted)
                    return;

                int damage = Utility.RandomMinMax(15, 25);
                // Use 100% poison damage for this attack.
                AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);

                // Optionally apply a debuff (here we freeze the target to simulate armor corrosion).
                target.SendAsciiMessage("The putrid acid corrodes your defenses!");
                target.Freeze(TimeSpan.FromSeconds(2.0));
            });
        }

        // -- Unique Ability: Poisonous Gas Cloud --
        // Releases a toxic gas cloud that poisons and damages any nearby enemy.
        public void PoisonCloud()
        {
            if (DateTime.UtcNow < _nextGasCloud)
                return;

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(3)) // Affects mobiles within 3 tiles.
            {
                if (m != this && CanBeHarmful(m))
                    targets.Add(m);
            }

            if (targets.Count == 0)
                return;

            // Play an animation and sound effect for the gas release.
            this.Animate(10, 5, 1, true, false, 0);
            this.PlaySound(0x5BD); // Gas cloud sound.

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                if (m is Mobile target)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    // Deal pure poison damage.
                    AOS.Damage(m, this, damage, 0, 0, 0, 100, 0);
                    // Apply deadly poison.
                    target.ApplyPoison(target, Poison.Deadly);
                    target.SendAsciiMessage("You cough as a deadly gas corrodes your lungs!");
                }
            }
            _nextGasCloud = DateTime.UtcNow + TimeSpan.FromSeconds(15.0);
        }

        // -- Unique Ability: Rampant Frenzy --
        // After landing several consecutive melee attacks, the Putrid Ridgeback unleashes a frenzied attack.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            _frenzyHits++;
            if (_frenzyHits >= 3)
            {
                this.PlaySound(0x56D); // Frenzy sound effect.
                if (defender != null && defender is Mobile target)
                {
                    DoHarmful(target);
                    int extraDamage = Utility.RandomMinMax(10, 15);
                    AOS.Damage(target, this, extraDamage, 100, 0, 0, 0, 0);
                    target.SendAsciiMessage("The Putrid Ridgeback's frenzied assault leaves you reeling!");
                }
                _frenzyHits = 0;
                _frenzyExpire = DateTime.UtcNow + TimeSpan.FromSeconds(5.0);
            }
        }

        // Reset frenzy counters if the effect expires.
        public void CheckFrenzy()
        {
            if (DateTime.UtcNow > _frenzyExpire)
            {
                _frenzyHits = 0;
                // If you maintain any damage multipliers, reset them here.
            }
        }

        // Override combat action to integrate our advanced abilities.
        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != this.Map || 
                !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
            {
                return;
            }

            // With a 20% chance, use Acid Spit if ready.
            if (DateTime.UtcNow >= _nextAcidSpit && Utility.RandomDouble() < 0.20)
            {
                AcidSpit(combatant);
                _nextAcidSpit = DateTime.UtcNow + TimeSpan.FromSeconds(12.0);
            }

            // With a 15% chance, use the Poison Cloud ability.
            if (Utility.RandomDouble() < 0.15)
            {
                PoisonCloud();
            }

            base.OnActionCombat();
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        // Miscellaneous creature properties.
        public override bool CanRummageCorpses { get { return true; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override int Meat { get { return 2; } }
        public override int Hides { get { return 20; } }
        public override HideType HideType { get { return HideType.Spined; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
