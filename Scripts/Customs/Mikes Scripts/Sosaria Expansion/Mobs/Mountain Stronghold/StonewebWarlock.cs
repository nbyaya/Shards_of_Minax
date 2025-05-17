using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("the remains of a stoneweb warlock")]
    public class StonewebWarlock : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextWebTime;
        private DateTime m_NextTremorTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // Unique Hue – a rocky, moss‑green
        private const int UniqueHue = 1763;

        [Constructable]
        public StonewebWarlock()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name        = "a Stoneweb Warlock";
            Body        = 85;
            BaseSoundID = 639;
            Hue         = UniqueHue;

            // ——— Stats ———
            SetStr(350, 400);
            SetDex(200, 240);
            SetInt(450, 500);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(15, 25);

            // Damage profile: 60% physical, 40% poison
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison,   40);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     30, 40);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   40, 50);

            // ——— Skills ———
            SetSkill(SkillName.EvalInt,     100.1, 115.0);
            SetSkill(SkillName.Magery,      100.1, 115.0);
            SetSkill(SkillName.MagicResist, 100.1, 115.0);
            SetSkill(SkillName.Poisoning,    90.1, 100.0);
            SetSkill(SkillName.Tactics,      90.1, 100.0);
            SetSkill(SkillName.Wrestling,    80.1,  95.0);

            Fame           = 18000;
            Karma         = -18000;
            VirtualArmor  = 70;
            ControlSlots  = 4;

            // Initialize timers
            m_NextWebTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextTremorTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;

            // Base reagents
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
        }

        // ——— Aura: Sticky Stoneweb — slows & damages stamina of those who step close ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

			if (m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && CanBeHarmful(m, false))
			{
				Mobile target = m as Mobile;
				if (target != null && SpellHelper.ValidIndirectTarget(this, target))
				{
					DoHarmful(target);

					// Slow effect
					target.SendMessage("Rocks and webs cling to your feet, slowing you!");
					target.PlaySound(0x55D);
					target.FixedParticles(0x376A, 10, 15, UniqueHue, EffectLayer.Waist);

					// Stamina drain
					int stamDrain = Utility.RandomMinMax(10, 20);
					target.Stam = Math.Max(0, target.Stam - stamDrain);
				}
			}


        }

        public override void OnThink()
        {
            base.OnThink();

            if (Map == null || !Alive || Combatant == null)
                return;

            var now = DateTime.UtcNow;

            if (now >= m_NextSummonTime && InRange(Combatant.Location, 12))
            {
                SummonStoneSpiders();
                m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            else if (now >= m_NextTremorTime && InRange(Combatant.Location, 8))
            {
                EarthTremorAttack();
                m_NextTremorTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextWebTime && InRange(Combatant.Location, 10))
            {
                StoneWebAttack();
                m_NextWebTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }

        // ——— Special Attack: Hurl Sticky Webs — spawns TrapWeb at the target ———
        public void StoneWebAttack()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*By stone and silk, I bind you!*");
                PlaySound(0x55E);
                target.FixedParticles(0x375A, 8, 15, UniqueHue, EffectLayer.Waist);

                // Drop a web trap under the target
                var web = new TrapWeb();
                web.Hue = UniqueHue;
                web.MoveToWorld(target.Location, target.Map);
            }
        }

        // ——— Special Attack: Earth Tremor — spawn EarthquakeTile hazards around ———
        public void EarthTremorAttack()
        {
            Say("*Feel the quake!*");
            PlaySound(0x2A8);

            // Create multiple tremor tiles in a radius 3
            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var tremor = new EarthquakeTile();
                tremor.Hue = UniqueHue;
                tremor.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // ——— Special Ability: Summon Stone Spiders — calls helpers to harass the party ———
        public void SummonStoneSpiders()
        {
            Say("*Rise, my stone‑forged brood!*");
            PlaySound(0x5A);

            for (int i = 0; i < 3; i++)
            {
                var spawnLoc = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z
                );

                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);

                var spider = new GiantSpider(); // Assumes a StoneSpider class exists
                spider.Hue = UniqueHue;
                spider.Controlled = false;
                spider.MoveToWorld(spawnLoc, Map);
            }
        }

        // ——— Death Effect: Explosive Webburst — spawns PoisonTiles and web hazards ———
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (this.Map == null)
				return;

            Say("*My work… is done…*");
            PlaySound(0x211);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            // Scatter a few poison‐web hazards
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-4, 4);
                int y = Y + Utility.RandomMinMax(-4, 4);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var poisoWeb = new PoisonTile();
                poisoWeb.Hue = UniqueHue;
                poisoWeb.MoveToWorld(new Point3D(x, y, z), Map);

                var web = new TrapWeb();
                web.Hue = UniqueHue;
                web.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // ——— Loot & Properties ———
        public override bool BleedImmune            => true;
        public override int TreasureMapLevel        => 6;
        public override double DispelDifficulty     => 135.0;
        public override double DispelFocus          => 65.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,        Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.MedScrolls,  Utility.RandomMinMax(2, 4));

            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new GownOfTheBloomguard());
        }

        public StonewebWarlock(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers on load
            m_NextWebTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextTremorTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSummonTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_LastLocation    = this.Location;
        }
    }
}
