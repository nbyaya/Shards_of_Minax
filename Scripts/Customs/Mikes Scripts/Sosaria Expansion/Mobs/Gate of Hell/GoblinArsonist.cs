using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For spell effects
using Server.Network; // For visual/audio effects
using System.Collections.Generic;
using Server.Spells.Fourth; // For FireFieldItem

namespace Server.Mobiles
{
	[CorpseName("a charred goblin corpse")]
	public class GoblinArsonist : BaseCreature
	{
		// Timers to prevent ability spam
		private DateTime m_NextFireballTime;
		private DateTime m_NextInfernoBurstTime;
		private Point3D m_LastLocation;

		// Unique fire hue for this monster (a bright, burning tint)
		private const int UniqueHue = 1175;

		[Constructable]
		public GoblinArsonist()
			: base(AIType.AI_Mage, FightMode.Closest, 10, 5, 0.2, 0.4)
		{
			Name = "a Goblin Arsonist";
			Body = 723; // Same as the base goblin
			BaseSoundID = 0x600;
			Hue = UniqueHue;


			// --- Significantly boosted stats ---
			SetStr(400, 450);
			SetDex(200, 250);
			SetInt(350, 400);

			SetHits(900, 1100);
			SetMana(300, 350);
			SetStam(200, 250);

			SetDamage(15, 20);

			// --- Damage Types: Mostly fire with a touch of physical ---
			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Fire, 80);

			// --- Resistances ---
			SetResistance(ResistanceType.Physical, 45, 55);
			SetResistance(ResistanceType.Fire, 80, 90);   // Strong fire resistance
			SetResistance(ResistanceType.Cold, -5, 0);      // Vulnerable to cold
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 30, 40);

			// --- Enhanced Skills ---
			SetSkill(SkillName.Magery, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 85.0, 95.0);
			SetSkill(SkillName.Tactics, 85.0, 95.0);
			SetSkill(SkillName.Wrestling, 95.0, 105.0);
			SetSkill(SkillName.EvalInt, 90.0, 100.0);

			Fame = 6000;
			Karma = -6000;
			VirtualArmor = 50;
			ControlSlots = 3;

			// --- Initialize ability timers ---
			m_NextFireballTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
			m_NextInfernoBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
			m_LastLocation = this.Location;

			// Standard loot & a little fire-themed extra
			PackItem(new SulfurousAsh(Utility.RandomMinMax(2, 5)));
			PackItem(new MandrakeRoot(Utility.RandomMinMax(1, 3)));
		}

		public GoblinArsonist(Serial serial)
			: base(serial)
		{ }

		// --- Sound Overrides (using the base goblin sounds) ---
		public override int GetAngerSound() { return 0x600; }
		public override int GetIdleSound() { return 0x600; }
		public override int GetAttackSound() { return 0x5FD; }
		public override int GetHurtSound() { return 0x5FF; }
		public override int GetDeathSound() { return 0x5FE; }

		public override bool CanRummageCorpses { get { return true; } }
		public override int TreasureMapLevel { get { return 2; } }
		public override int Meat { get { return 1; } }
		public override TribeType Tribe { get { return TribeType.GreenGoblin; } }

		// --- Unique Passive Ability: Flame Trail ---
		// Every time the Goblin Arsonist moves, it leaves a short-lived fire field at its previous location.
		public override void OnMovement(Mobile m, Point3D oldLocation)
		{
			// When this creature moves (its location changes) create a flame trail effect at the old location.
			if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
			{
				Point3D trailLocation = m_LastLocation;
				m_LastLocation = this.Location; // Update last location

				int itemID = 0x398C; // Fire field graphic
				TimeSpan duration = TimeSpan.FromSeconds(4 + Utility.Random(3)); // Lasts about 4-6 seconds
				int damage = 2;

				var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, trailLocation, this, this.Map, duration, damage);
				Effects.SendLocationParticles(EffectItem.Create(trailLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0);
			}
			base.OnMovement(m, oldLocation);
		}

		// --- Advanced Ability Handling in OnThink ---
		public override void OnThink()
		{
			base.OnThink();

			// Ensure we have a valid combatant and map
			if (Combatant == null || Map == null || Map == Map.Internal)
				return;

			// Attempt to use Fireball Toss if its cooldown is ready and the Combatant is in range.
			if (DateTime.UtcNow >= m_NextFireballTime && InRange(Combatant.Location, 8))
			{
				// Check if Combatant is a Mobile before accessing specific properties.
				if (Combatant is Mobile target)
				{
					FireballAttack(target);
					m_NextFireballTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
				}
			}
			// Attempt to use a close-range Inferno Burst if conditions are met.
			else if (DateTime.UtcNow >= m_NextInfernoBurstTime && InRange(Combatant.Location, 4))
			{
				InfernoBurstAttack();
				m_NextInfernoBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
			}
		}

		// --- Unique Ability: Fireball Toss ---
		// Hurls a quick fireball toward the primary target.
		public void FireballAttack(Mobile target)
		{
			if (Map == null || target == null)
				return;

			// Visual: Launch a fireball projectile effect
			Point3D startPoint = this.Location;
			Point3D targetLocation = target.Location;

			Effects.SendMovingParticles(
				new Entity(Serial.Zero, startPoint, this.Map),
				new Entity(Serial.Zero, targetLocation, this.Map),
				0x36D4, // Fireball graphic
				5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

			// Play fireball sound effect
			PlaySound(0x208);

			// Delay damage slightly to match the projectile travel
			Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
			{
				// Ensure target is still valid for damage
				if (Map == null || target.Deleted)
					return;

				Effects.SendLocationParticles(
					EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration),
					0x3709, 10, 30, UniqueHue, 0, 5016, 0);

				if (CanBeHarmful(target, false) && SpellHelper.ValidIndirectTarget(this, target))
				{
					DoHarmful(target);
					int damage = Utility.RandomMinMax(25, 35); // Moderate fire damage
					// Deal pure fire damage
					AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
				}
			});
		}

		// --- Unique Ability: Inferno Burst ---
		// Releases a burst of flame around itself, damaging all nearby valid targets.
		public void InfernoBurstAttack()
		{
			if (Map == null)
				return;

			PlaySound(0x208); // Fire-like sound effect
			// Visual: Create a burst effect around the Goblin Arsonist
			Effects.SendLocationParticles(
				EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
				0x3709, 10, 40, UniqueHue, 0, 5029, 0);

			List<Mobile> targets = new List<Mobile>();
			IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 4); // 4-tile radius AoE

			foreach (Mobile m in eable)
			{
				if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
				{
					targets.Add(m);
				}
			}
			eable.Free();

			// Deal significant AoE fire damage to all valid targets
			foreach (Mobile target in targets)
			{
				DoHarmful(target);
				int damage = Utility.RandomMinMax(35, 50);
				AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
				target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head); // Impact effect on target
			}
		}

		// --- Death Explosion ---
		// On death the Goblin Arsonist splinters its burning essence, leaving behind multiple small lava fields.
		public override void OnDeath(Container c)
		{
			if (Map == null)
			{
				base.OnDeath(c);
				return;
			}

			int lavaTilesToDrop = 5; // Number of lava fields to spawn
			List<Point3D> effectLocations = new List<Point3D>();

			for (int i = 0; i < lavaTilesToDrop; i++)
			{
				int xOffset = Utility.RandomMinMax(-2, 2);
				int yOffset = Utility.RandomMinMax(-2, 2);
				// Avoid spawning directly on top every time
				if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop - 1)
					xOffset = Utility.RandomBool() ? 1 : -1;

				Point3D lavaLocation = new Point3D(X + xOffset, Y + yOffset, Z);

				// Validate the location for a fire field
				if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
				{
					lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
					if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
						continue;
				}

				effectLocations.Add(lavaLocation);
				// Spawn a HotLavaTile at the location (assuming HotLavaTile is defined elsewhere)
				HotLavaTile droppedLava = new HotLavaTile();
				droppedLava.Hue = UniqueHue;
				droppedLava.MoveToWorld(lavaLocation, this.Map);

				Effects.SendLocationParticles(
					EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration),
					0x3709, 10, 20, UniqueHue, 0, 5016, 0);
			}

			// Create a focal explosion effect
			Point3D deathLocation = this.Location;
			if (effectLocations.Count > 0)
				deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

			Effects.PlaySound(deathLocation, this.Map, 0x218);
			Effects.SendLocationParticles(
				EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration),
				0x3709, 10, 40, UniqueHue, 0, 5052, 0);

			base.OnDeath(c);
		}

		// --- Loot Generation ---
		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich, 2);
			AddLoot(LootPack.Average);
			// Chance for rare, fire-themed resource
			if (Utility.RandomDouble() < 0.05)
			{
				PackItem(new DaemonBone(Utility.RandomMinMax(1, 3)));
			}
		}

		// --- Serialization ---
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			// Reinitialize timers on load/restart
			m_NextFireballTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
			m_NextInfernoBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
			m_LastLocation = this.Location;
		}
	}
}
