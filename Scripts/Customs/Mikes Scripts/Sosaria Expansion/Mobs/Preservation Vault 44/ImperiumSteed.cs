using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an imperium steed corpse")]
    public class ImperiumSteed : BaseCreature
    {
        // Cooldowns
        private DateTime m_NextChargeTime;
        private DateTime m_NextHoofstormTime;
        private DateTime m_NextDominationTime;
        private Point3D m_LastLocation;

        // Unique metallic‑gold hue
        private const int UniqueHue = 1157;

        [Constructable]
        public ImperiumSteed() 
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "an Imperium Steed";
            Body = 0x75;                 // same as SilverSteed
            BaseSoundID = 0x3EA8;        // same as SilverSteed
            Hue = UniqueHue;

            // Stats
            SetStr(550, 650);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(2000, 2300);
            SetStam(300, 350);
            SetMana(200, 250);

            SetDamage(20, 30);

            // Damage types (blend physical & energy)
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Energy, 60);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Skills
            SetSkill(SkillName.Wrestling, 100.1, 115.0);
            SetSkill(SkillName.Tactics,   100.1, 115.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 100;
            ControlSlots = 5;

            // Initialize cooldowns
            m_NextChargeTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextHoofstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextDominationTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            m_LastLocation = Location;

            // Starter loot
            PackItem(new Gold(Utility.RandomMinMax(1000, 2000)));
            PackItem(new VoidCore());
        }

        // Aura: whenever a player comes within 3 tiles, deal energy damage
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (Alive && m != this && m.Map == Map && InRange(m, 3) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int dmg = Utility.RandomMinMax(10, 20);
                    AOS.Damage(target, this, dmg, 0, 0, 0, 0, 100);
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Charge Attack
            if (now >= m_NextChargeTime && InRange(Combatant.Location, 12))
            {
                AstralCharge();
                m_NextChargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                return;
            }

            // Hoofstorm
            if (now >= m_NextHoofstormTime && InRange(Combatant.Location, 8))
            {
                SpectralHoofstorm();
                m_NextHoofstormTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
                return;
            }

            // Domination Aura
            if (now >= m_NextDominationTime && InRange(Combatant.Location, 6))
            {
                DominationGaze();
                m_NextDominationTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Charge: high‑speed dash, knockback & damage ---
        public void AstralCharge()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*The ground trembles!*");
            PlaySound(0x2F1);

            // Dash visual
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, Location, Map),
                new Entity(Serial.Zero, target.Location, Map),
                0x3778, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);

            // Deal damage & knockback
            DoHarmful(target);
            int dmg = Utility.RandomMinMax(40, 60);
            AOS.Damage(target, this, dmg, 100, 0, 0, 0, 0);

            // Knockback
            var pushVec = new Point3D(
                target.X + (target.X - X),
                target.Y + (target.Y - Y),
                target.Z);
            target.MoveToWorld(pushVec, Map);
        }

        // --- Hoofstorm: 360° AoE energy shards + quicksand tiles ---
        public void SpectralHoofstorm()
        {
            Say("*Feel the storm!*");
            PlaySound(0x212);

            // Particle burst
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 12, 20, UniqueHue, 0, 5052, 0);

            // Spawn Quicksand tiles in a radius
            for (int dx = -2; dx <= 2; dx++)
            for (int dy = -2; dy <= 2; dy++)
            {
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new QuicksandTile { Hue = UniqueHue };
                    tile.MoveToWorld(loc, Map);
                }
            }

            // Damage all in range
            var list = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in list)
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile t)
                {
                    DoHarmful(t);
                    int dmg = Utility.RandomMinMax(30, 50);
                    AOS.Damage(t, this, dmg, 0, 0, 100, 0, 0);
                }
            }
        }

        // --- Domination: fear + light stun ---
        public void DominationGaze()
        {
            Say("*Submit to my will!*");
            PlaySound(0x22F);

            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                DoHarmful(target);

                // Fear effect
                target.SendMessage(0x22, "You are overwhelmed with terror!");
                target.FixedParticles(0x375A, 10, 15, 5012, UniqueHue, 0, EffectLayer.Head);

                // Stun
                target.Freeze(TimeSpan.FromSeconds(3));
            }
        }

        // --- On death: spawn LightningStormTiles + explosion ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            Say("*My reign ends...*");
            PlaySound(0x211);

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 15, 30, UniqueHue, 0, 5052, 0);

            // Scatter 4–6 LightningStormTiles
            int count = Utility.RandomMinMax(4, 6);
            for (int i = 0; i < count; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new LightningStormTile { Hue = UniqueHue };
                tile.MoveToWorld(loc, Map);
            }
        }

        // Loot & serialization
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new BargainBreaker());
        }

        public ImperiumSteed(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns
            m_NextChargeTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextHoofstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextDominationTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            m_LastLocation = Location;
        }
    }
}
