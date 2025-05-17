using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Spells;           // For ApplyPoison
using Server.Mobiles;
using Server.Targeting;       // For AOE targeting

namespace Server.Mobiles
{
    [CorpseName("a blighted rat corpse")]
    public class Blightrun : BaseCreature
    {
        private DateTime m_NextToxicBurst;

        [Constructable]
        public Blightrun()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Blightrun";
            Body = 0xD7;             // Plague Rat body
            BaseSoundID = 0x188;     // Plague Rat sounds
            Hue = 0x8A1;             // Sickly green/purple hue

            // Stats
            SetStr(250, 300);
            SetDex(150, 200);
            SetInt(75, 100);

            SetHits(800, 950);
            SetMana(25, 50);
            SetStam(150, 200);

            SetDamage(12, 18);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Poison, 70);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.MagicResist, 80.0, 95.0);
            SetSkill(SkillName.Tactics,      90.0, 100.0);
            SetSkill(SkillName.Wrestling,    95.0, 110.0);
            SetSkill(SkillName.Anatomy,      75.0,  85.0);

            Fame = 8000;
            Karma = -8000;
            VirtualArmor = 50;
        }

        public Blightrun(Serial serial)
            : base(serial)
        {
        }

        // === AOE Toxic Burst ===
        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant == null || DateTime.UtcNow < m_NextToxicBurst)
                return;

            if (Utility.RandomDouble() < 0.25)  // 25% chance
            {
                m_NextToxicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
				Mobile m = Combatant as Mobile;
				if (m != null)
				{
					DoToxicBurst(m);
				}

            }
        }

        private void DoToxicBurst(Mobile target)
        {
            if (target == null || target.Deleted || Deleted || Map == null)
                return;

            Say("*The Blightrun prepares to unleash a toxic cloud!*");
            Animate(10, 5, 1, true, false, 0);

            PlaySound(0x188);

            Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
            {
                if (Deleted || Map == null)
                    return;

                Say("*BLIGHT!*");
                Effects.PlaySound(Location, Map, 0x20F);

                foreach (var m in GetMobilesInRange(3))
                {
                    if (m == this || !CanBeHarmful(m))
                        continue;

                    DoHarmful(m);

                    int dmg = Utility.RandomMinMax(30, 50);
                    AOS.Damage(m, this, dmg, 0, 0, 0, 0, 100);
                    m.ApplyPoison(this, Poison.Greater);
                    m.SendAsciiMessage("You are engulfed by the Blightrun's toxic burst!");
                }

                // Lingering cloud
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x36B0, 10, 30, Hue, 0, 9502, 0
                );
            });
        }

        // === On Gave Melee Attack ===
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() <= 0.30)
            {
                defender.ApplyPoison(this, Poison.Regular);
                defender.SendAsciiMessage("You feel the Blightrun's bite inject venom!");
                PlaySound(0x20E);
            }
        }

        // === On Got Melee Attack ===
        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (Utility.RandomDouble() <= 0.15)
            {
                int decay = Utility.RandomMinMax(5, 10);
                AOS.Damage(attacker, this, decay, 0, 0, 0, 100, 0);
                attacker.SendAsciiMessage("Your flesh feels like it's decaying from the Blightrun's touch!");
                PlaySound(0x541);
            }
        }

        // === Loot & Resources ===
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);  // Very high tier :contentReference[oaicite:2]{index=2}
            AddLoot(LootPack.Gems);
            AddLoot(LootPack.Potions);
            AddLoot(LootPack.Rich);

            // Thematic ingredients
            PackItem(new Nightshade(Utility.RandomMinMax(5, 20)));

            // 1–3 Poison Potions
            int count = Utility.RandomMinMax(1, 3);
            for (int i = 0; i < count; i++)
                PackItem(new PoisonPotion());

            // 1% chance for unique
            if (Utility.RandomDouble() < 0.01)
                PackItem(new BlightedRatTail());
        }

        public override int Meat  => 2;
        public override int Hides => 10;
        public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish | FoodType.Eggs;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 2;

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

    // === Unique Item Tail ===
    public class BlightedRatTail : Item
    {
        [Constructable]
        public BlightedRatTail() : base(0xF91)
        {
            Name = "a blighted rat tail";
            Hue = 0x8A1;
            LootType = LootType.Regular;  // no 'Rare' enum in ServUO :contentReference[oaicite:3]{index=3}
        }

        public BlightedRatTail(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
