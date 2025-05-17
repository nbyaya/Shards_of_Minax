using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a duskwyr corpse")]
    public class Duskwyr : BaseCreature
    {
        private static readonly int UniqueHue = 0x8A5;

        private DateTime _NextDuskwyrHowl;
        private DateTime _NextShadowSwipe;
        private DateTime _NextTwilightCloak;

        [Constructable]
        public Duskwyr()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a duskwyr";
            Body = 23;
            BaseSoundID = 0xE5;
            Hue = UniqueHue;

            SetStr(300, 400);
            SetDex(150, 200);
            SetInt(100, 150);

            SetHits(800, 1000);
            SetMana(200, 300);
            SetStam(150, 200);

            SetDamage(18, 25);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 80.1, 95.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 95.1, 105.0);
            SetSkill(SkillName.Anatomy, 70.0, 80.0);
            SetSkill(SkillName.Stealth, 50.0, 70.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 40;

            Tamable = false;
            ControlSlots = 5;
            MinTameSkill = 120.0;

            PackItem(new Bone(5));
            PackItem(new Leather(10));
            PackGold(500, 1000);
        }

        public Duskwyr(Serial serial) : base(serial) { }

        public void DuskwyrHowl()
        {
            if (Deleted || !Alive) return;

            _NextDuskwyrHowl = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            PlaySound(0x1D4);
            Say("*A chilling howl builds in the duskwyr's throat!*");

            Timer.DelayCall(TimeSpan.FromSeconds(3.0), DuskwyrHowl_Finish, this);
        }

        public static void DuskwyrHowl_Finish(object state)
        {
            var caller = state as Duskwyr;
            if (caller == null || caller.Deleted || !caller.Alive) return;

            caller.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
            caller.PlaySound(0x1E4);

            var targets = new List<Mobile>();
            foreach (Mobile m in caller.GetMobilesInRange(6))
            {
                if (m != caller && caller.CanBeHarmful(m))
                    targets.Add(m);
            }

            foreach (Mobile target in targets)
            {
                caller.DoHarmful(target);
                target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                target.PlaySound(0x204);
                target.SendMessage("The duskwyr's howl chills you to the bone!");

                var stamLoss = Utility.RandomMinMax(20, 40);
                target.Stam = Math.Max(0, target.Stam - stamLoss);

                // Simulate curse (apply stat mods directly)
                TimeSpan duration = TimeSpan.FromSeconds(30);
                target.AddStatMod(new StatMod(StatType.Str, "DuskwyrCurseStr", -10, duration));
                target.AddStatMod(new StatMod(StatType.Dex, "DuskwyrCurseDex", -10, duration));
                target.AddStatMod(new StatMod(StatType.Int, "DuskwyrCurseInt", -10, duration));

                AOS.Damage(target, caller, Utility.RandomMinMax(25, 40), 0, 0, 50, 0, 50);
            }
        }

        public void ShadowSwipe(Mobile target)
        {
            if (Deleted || !Alive || target == null || target.Deleted || !CanBeHarmful(target))
                return;

            _NextShadowSwipe = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            DoHarmful(target);

            target.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
            target.PlaySound(0x476);

            int drain = Utility.RandomMinMax(10, 25);
            bool drained = false;

            if (target.Mana > 0)
            {
                var used = Math.Min(drain, target.Mana);
                target.Mana -= used;
                drained = true;
            }
            if (target.Stam > 0)
            {
                var used = Math.Min(drain, target.Stam);
                target.Stam -= used;
                drained = true;
            }
            if (drained)
                target.SendMessage("You feel your energy draining from the duskwyr's swipe!");
        }

        public void ActivateTwilightCloak()
        {
            if (Deleted || !Alive) return;

            _NextTwilightCloak = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 40));
            FixedParticles(0x3709, 1, 30, 9963, 1, 5, EffectLayer.Waist);
            PlaySound(0x20F);
            Say("*The duskwyr is shrouded in twilight!*");

            // Placeholder for resistance or stealth boost
        }

/* 		public override void OnActionCombat()
		{
			Mobile combatant = Combatant; // Ensure it's a Mobile

			if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
			{
				base.OnActionCombat();
				return;
			}

			if (DateTime.UtcNow >= _NextDuskwyrHowl && InRange(combatant, 8))
			{
				DuskwyrHowl();
			}
			else if (DateTime.UtcNow >= _NextTwilightCloak)
			{
				ActivateTwilightCloak();
			}
			else
			{
				if (DateTime.UtcNow >= _NextShadowSwipe && Utility.RandomDouble() < 0.3)
				{
					ShadowSwipe(combatant);
				}
				else
				{
					Animate(1, 1, 0, true, false, 0);  // Attack animation (1 = Attack)
					Attack(combatant);
				}
			}
		} */


        public override bool IsEnemy(Mobile m)
        {
            if (m is BaseCreature bc && bc.IsMonster && m.Karma > 0)
                return true;
            if (m.Player)
                return true;

            return base.IsEnemy(m);
        }

        public override int Meat => 2;
        public override int Hides => 15;
        public override HideType HideType => HideType.Barbed;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Canine;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.SuperBoss);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Potions, 3);

            if (Utility.RandomDouble() < 0.05)
                PackItem(new DuskShard());

            if (Utility.RandomDouble() < 0.01)
                PackItem(new DuskwyrFang());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            _NextDuskwyrHowl = DateTime.UtcNow;
            _NextShadowSwipe = DateTime.UtcNow;
            _NextTwilightCloak = DateTime.UtcNow;
        }
    }

    // ------------------------------------------------------------------------
    // Custom shard drop
    public class DuskShard : Item
    {
        [Constructable]
        public DuskShard() : base(0xF8F)
        {
            Name = "dusk shard";
            Hue = 0x455;
            LootType = LootType.Regular;
        }

        public DuskShard(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }

    // ------------------------------------------------------------------------
    // Custom fang drop
    public class DuskwyrFang : Item
    {
        [Constructable]
        public DuskwyrFang() : base(0x1F14) // pick an appropriate itemID
        {
            Name = "duskwyr fang";
            Hue = 0x455;
            LootType = LootType.Regular;
        }

        public DuskwyrFang(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }
}
